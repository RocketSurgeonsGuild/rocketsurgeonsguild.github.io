using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Utilities.Collections;
using Wyam.Core.Execution;
using System.Diagnostics;
using System.Threading.Tasks;
using Octokit.Reactive;
using Octokit;
using System.Reactive.Linq;
using System.IO;
using Buildalyzer;
using System.Reactive.Concurrency;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using Nuke.Common.IO;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tooling;
using Wyam.Common.Meta;
using Nuke.Common.Git;
using Nuke.Common.Tools.Git;
using static Nuke.Common.Tools.Git.GitTasks;

[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Branch to deploy to")] public readonly string DeployToBranch = "local";

    public IEnumerable<PackageSpec> PackageSpecs => GlobFiles(RootDirectory / "packages", "*.yml", "*.yaml")
       .Select(File.ReadAllText)
       .Select(x => new DeserializerBuilder().Build().Deserialize<PackageSpec>(x))
       .ToArray();

    Target Clean => _ => _
       .Executes(() => { });

    Target Restore => _ => _
       .DependsOn(RefreshPackages)
       .Executes(
            () =>
            {
                var projectDirectory = TemporaryDirectory / "_project";
                var packagesDirectory = TemporaryDirectory / "_packages";
                var project = projectDirectory / "project.csproj";
                EnsureExistingDirectory(projectDirectory);
                EnsureExistingDirectory(packagesDirectory);
                if (!FileExists(project))
                {
                    System.IO.File.WriteAllText(
                        project,
                        @"
                    <Project Sdk=""Microsoft.NET.Sdk"">
                        <PropertyGroup>
                            <TargetFramework>netcoreapp3.1</TargetFramework>
                        </PropertyGroup>
                    </Project>"
                    );

                    foreach (var packageSpec in PackageSpecs)
                    {
                        DotNet($"add package {packageSpec.Name} --no-restore", projectDirectory);
                    }
                }

                try
                {
                    DotNetRestore(
                        x => x
                           .EnableNoDependencies()
                           .SetPackageDirectory(packagesDirectory)
                           .SetWorkingDirectory(projectDirectory)
                    );
                }
                catch { }
            }
        );

    Target Compile => _ => _
       .DependsOn(Restore)
       .Executes(
            () =>
            {
                Wyam.Common.Tracing.Trace.AddListener(new NukeTraceListener());
                Wyam.Common.Tracing.Trace.Level = SourceLevels.All;
                var engine = new Engine();
                new WyamConfiguration(engine, this);
                engine.Execute();
            }
        );

    Target Preview => _ => _
       .Executes(
            () =>
            {
                Wyam.Common.Tracing.Trace.AddListener(new NukeTraceListener());
                Wyam.Common.Tracing.Trace.Level = SourceLevels.All;
                PreviewServer.Preview(
                    () =>
                    {
                        var engine = new Engine();
                        engine.Settings[Keys.CleanOutputPath] = false;
                        new WyamConfiguration(engine, this);
                        return engine;
                    },
                    this
                );
            }
        );

    [Parameter("Github Token - To use when syncing packages")]
    readonly string GithubToken = EnvironmentInfo.GetVariable<string>("GITHUB_TOKEN") ?? string.Empty;

    Target RefreshPackages => _ => _
       .Executes(
            async () =>
            {
                var client = string.IsNullOrWhiteSpace(GithubToken)
                    ? new ObservableGitHubClient(new ProductHeaderValue("internaltooling.to.update.repo"))
                    : new ObservableGitHubClient(
                        new ProductHeaderValue("internaltooling.to.update.repo"),
                        new Octokit.Internal.InMemoryCredentialStore(new Credentials(GithubToken))
                    );
                var repos = client.Repository.GetAllForOrg("RocketSurgeonsGuild")
                   .Where(repo => !repo.Archived)
                   .Where(repo => IsLocalBuild && !DirectoryExists(TemporaryDirectory / repo.Name) || !IsLocalBuild);

                await repos
                   .ObserveOn(TaskPoolScheduler.Default)
                   .Select(
                        repo =>
                        {
                            var path = TemporaryDirectory / repo.Name;
                            Git($"clone --depth 1 --single-branch {repo.CloneUrl} {path}", logOutput: false);
                            return ( path, repo );
                        }
                    )
                   .ForEachAsync(x => { });

                var solutions = repos
                   .Select(repo => ( path: TemporaryDirectory / repo.Name, repo ))
                   .SelectMany(
                        x =>
                            Directory.EnumerateFiles(x.path, "*.sln")
                               .ToObservable()
                               .Select(solutionFilePath => ( x.path, x.repo, solutionFilePath ))
                    );

                var projects = solutions
                   .Select(x => ( x.path, x.repo, x.solutionFilePath, new AnalyzerManager(x.solutionFilePath) ))
                   .SelectMany(
                        x =>
                        {
                            var (path, repo, solutionFilePath, analyzerManager) = x;
                            return analyzerManager.Projects
                               .Where(z => z.Key.Contains("/src/") || z.Key.Contains(@"\src\"))
                               .Select(
                                    project =>
                                    {
                                        return ( path, repo, solutionFilePath, analyzerManager,
                                                 projectFilePath: project.Key, project: project.Value,
                                                 projectBuild: project.Value.Build().FirstOrDefault() );
                                    }
                                )
                               .ToObservable();
                        }
                    )
                   .Do(
                        z =>
                        {
                            if (z.projectBuild == null)
                            {
                                Logger.Warn("Unable to build project {0}", z.projectFilePath);
                            }
                        })
                   .Where(z => z.projectBuild != null)
                   .Distinct(x => x.projectFilePath);

                await projects
                   .ObserveOn(TaskPoolScheduler.Default)
                   .ForEachAsync(
                        x =>
                        {
                            var assemblyTitle = x.projectBuild.GetProperty("AssemblyTitle");
                            var projectUrl = x.projectBuild.GetProperty("PackageProjectUrl");
                            var authors = ( x.projectBuild.GetProperty("Authors") ?? string.Empty ).Split(',');
                            var copyright = x.projectBuild.GetProperty("Copyright");

                            var assemblyName = x.projectBuild.GetProperty("AssemblyName");
                            var tags = ( x.projectBuild.GetProperty("PackageTags") ?? string.Empty ).Split(';');
                            var targetFrameworks =
                                ( x.projectBuild.GetProperty("TargetFrameworks") ??
                                    x.projectBuild.GetProperty("TargetFramework") ?? string.Empty ).Split(';');
                            var description = x.projectBuild.GetProperty("PackageDescription");

                            var serializer = new SerializerBuilder().Build();
                            var categories = new List<string>();
                            if (x.repo.Name.Contains(".Extensions"))
                            {
                                categories.Add("Extensions");
                            }

                            if (assemblyName.Contains(".Extensions"))
                            {
                                categories.Add("Extensions");
                            }

                            if (assemblyName.Contains(".Abstractions"))
                            {
                                categories.Add("Abstractions");
                            }

                            if (assemblyName.Contains(".AspNetCore"))
                            {
                                categories.Add("AspNetCore");
                            }

                            if (assemblyName.Contains(".Hosting"))
                            {
                                categories.Add("Hosting");
                            }

                            var yaml = serializer.Serialize(
                                new
                                {
                                    Name = assemblyName,
                                    NuGet = assemblyName,
                                    Assemblies = new[] { $"/{assemblyName.ToLowerInvariant()}/**/{assemblyName}.dll" },
                                    Repository = x.repo.HtmlUrl,
                                    GitName = x.repo.Name,
                                    GitUrl = x.repo.CloneUrl,
                                    Author = x.projectBuild.GetProperty("Authors") ?? "",
                                    Description = description,
                                    Categories = categories.Concat(
                                        new[]
                                        {
                                            x.repo.Name.Replace(".Extensions", "").Replace(".Abstractions", ""),
                                            assemblyName.Replace(".Extensions", "").Replace(".Abstractions", "")
                                               .Split('.').Last()
                                        }
                                    ).Distinct().OrderBy(z => z)
                                }
                            );

                            EnsureExistingDirectory(RootDirectory / "packages");
                            File.WriteAllText(
                                Path.Combine(RootDirectory / "packages", assemblyName.ToLower() + ".yml"),
                                yaml
                            );
                        }
                    );
            }
        );
}

class PackageSpec
{
    public string Name { get; set; }
    public string NuGet { get; set; }
    public bool Prerelease { get; set; }
    public List<string> Assemblies { get; set; }
    public string Repository { get; set; }
    public string GitName { get; set; }
    public string GitUrl { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public List<string> Categories { get; set; }
}