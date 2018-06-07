#tool "nuget:https://api.nuget.org/v3/index.json?package=KuduSync.NET&version=1.3.1"
#tool "nuget:https://api.nuget.org/v3/index.json?package=Wyam&version=1.2.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Wyam&version=1.2.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Git&version=0.16.1"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Kudu&version=0.5.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Yaml&version=2.0.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=YamlDotNet&version=4.2.1"


//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");


// Define variables
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest       = AppVeyor.Environment.PullRequest.IsPullRequest;
var accessToken         = EnvironmentVariable("git_access_token");
var deployRemote        = EnvironmentVariable("git_deploy_remote");
var currentBranch       = isRunningOnAppVeyor ? BuildSystem.AppVeyor.Environment.Repository.Branch : GitBranchCurrent("./").FriendlyName;
var deployBranch        = "master";

// Define directories.
var releaseDir          = Directory("./release");
var sourceDir           = releaseDir + Directory("repo");
var packageDir          = releaseDir + Directory("pkgs");
var outputPath          = MakeAbsolute(Directory("./output"));
var rootPublishFolder   = MakeAbsolute(Directory("publish"));

class PackageSpec
{
    public string Name { get; set; }
    public string NuGet { get; set; }
    public bool Prerelease { get; set; }
    public List<string> Assemblies { get; set; }
    public string Repository { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public List<string> Categories { get; set; }
}

List<PackageSpec> packageSpecs = new List<PackageSpec>();

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("CleanSource")
    .Does(() =>
    {
        if(DirectoryExists(sourceDir))
        {
            DeleteDirectory(sourceDir, new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            });
        }
        EnsureDirectoryExists(releaseDir);
    });

Task("GetSource")
    .IsDependentOn("CleanSource")
    .Does(() =>
    {
        GitClone("https://github.com/RocketSurgeonsGuild/rocketsurgeonsguild.github.io.git", sourceDir, new GitCloneSettings{ BranchName = "master" });
    });

Task("CleanNugetPackages")
    .Does(() =>
{
    CleanDirectory(packageDir);
});

Task("GetPackageSpecs")
    .Does(() =>
{
    var packageSpecFiles = GetFiles("./pkgs/*.yml");
    packageSpecs
        .AddRange(packageSpecFiles
            .Select(x =>
            {
                Information("Deserializing package YAML from " + x);
                return DeserializeYamlFromFile<PackageSpec>(x);
            })
        );
});

Task("GetNugetPackages")
    .IsDependentOn("CleanNugetPackages")
    .IsDependentOn("GetPackageSpecs")
    .Does(() =>
    {
        DirectoryPath   packagesPath        = MakeAbsolute(Directory("./output")).Combine("packages");
        Parallel.ForEach(
            packageSpecs.Where(x => !string.IsNullOrEmpty(x.NuGet)),
            packageSpec => {
                Information("Installing package " + packageSpec.NuGet);
                NuGetInstall(packageSpec.NuGet,
                    new NuGetInstallSettings
                    {
                        OutputDirectory = packageDir,
                        Prerelease = packageSpec.Prerelease,
                        Verbosity = NuGetVerbosity.Quiet,
                        Source = new [] { "https://api.nuget.org/v3/index.json" },
                        NoCache = true,
                        EnvironmentVariables    = new Dictionary<string, string>{
                                                        {"EnableNuGetPackageRestore", "true"},
                                                        {"NUGET_XMLDOC_MODE", "None"},
                                                        {"NUGET_PACKAGES", packagesPath.FullPath},
                                                        {"NUGET_EXE",  Context.Tools.Resolve("nuget.exe").FullPath }
                                                  }
                    });
        });
    });

Task("Build")
    .IsDependentOn("GetArtifacts")
    .Does(() =>
    {
        Wyam(new WyamSettings
        {
            Recipe = "Docs",
            Theme = "Samson",
            UpdatePackages = true,
            Settings = new Dictionary<string, object>
            {
                { "AssemblyFiles",  packageSpecs.Where(x => x.Assemblies != null).SelectMany(x => x.Assemblies).Select(x => "../release/pkgs" + x) }
            }
        });
    });

Task("Preview")
    .IsDependentOn("GetPackageSpecs")
    .Does(() =>
    {
        Wyam(new WyamSettings
        {
            Recipe = "Docs",
            Theme = "Samson",
            UpdatePackages = true,
            Preview = true,
            Watch = true,
            Settings = new Dictionary<string, object>
            {
                { "AssemblyFiles",  packageSpecs.Where(x => x.Assemblies != null).SelectMany(x => x.Assemblies).Select(x => "../release/pkgs" + x) }
            }
        });
    });

Task("Copy-Repo-Files")
    .IsDependentOn("Build")
    .Does(() =>
    {
        CopyFiles(
            new FilePath[] {
                "LICENSE",
                "README.md",
                "appveyor.yml"
            },
            "./output"
            );
    });

Task("Deploy")
    .WithCriteria(isRunningOnAppVeyor)
    .WithCriteria(!isPullRequest)
    .WithCriteria(!string.IsNullOrEmpty(accessToken))
    .WithCriteria(currentBranch == "dev")
    .WithCriteria(!string.IsNullOrEmpty(deployRemote))
    .WithCriteria(!string.IsNullOrEmpty(deployBranch))
    .IsDependentOn("Copy-Repo-Files")
    .Does(() =>
    {
        EnsureDirectoryExists(rootPublishFolder);
        var sourceCommit = GitLogTip("./");
        var publishFolder = rootPublishFolder.Combine(DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        Information("Getting publish branch {0}...", deployBranch);
        GitClone(deployRemote, publishFolder, new GitCloneSettings{ BranchName = deployBranch });

        Information("Sync output files...");
        Kudu.Sync(outputPath, publishFolder, new KuduSyncSettings {
            PathsToIgnore = new []{ ".git", ".gitignore" }
        });

        Information("Stage all changes...");
        GitAddAll(publishFolder);

        Information("Commit all changes...");
        GitCommit(
            publishFolder,
            sourceCommit.Committer.Name,
            sourceCommit.Committer.Email,
            string.Format("AppVeyor Publish: {0}\r\n{1}", sourceCommit.Sha, sourceCommit.Message)
            );

        Information("Pushing all changes...");
        GitPush(publishFolder, accessToken, "x-oauth-basic", deployBranch);
    });

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

Task("GetArtifacts")
    .IsDependentOn("GetSource")
    .IsDependentOn("GetNugetPackages");

Task("AppVeyor")
    .IsDependentOn("Deploy");

RunTarget(target);