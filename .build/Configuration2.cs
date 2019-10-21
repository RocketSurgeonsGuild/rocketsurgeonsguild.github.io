using Nuke.Common;
using Wyam.Core.Execution;
using Wyam.Common.Execution;
using Wyam.Common.IO;
using Wyam.Docs;
using Wyam.Common.Meta;
using Wyam.Core.Modules.IO;
using Wyam.Yaml;
using Wyam.Core.Modules.Control;
using Wyam.Core.Modules.Metadata;
using Wyam.Razor;
using Wyam.Configuration;
using Nuke.Common.IO;
using static Nuke.Common.IO.PathConstruction;
using System.Linq;
using Wyam.Html;
using Wyam.Web;
using Wyam.Feeds;
using Wyam.CodeAnalysis;
using Wyam.Common.Modules;
using Wyam.Json;
using Microsoft.CodeAnalysis;
using System;
using Wyam.Common.Documents;
using System.Collections.Generic;
using Wyam.Common.Configuration;
using Humanizer;

class WyamConfiguration : ConfigurationEngineBase
{
    public WyamConfiguration(Engine engine, Build build) : base(engine)
    {
        var configurator = new Configurator(engine);
        configurator.Recipe = new Wyam.Docs.Docs();
        configurator.Theme = "Samson";
        configurator.Configure("");
        configurator.AssemblyLoader.DirectAssemblies.Add(typeof(HtmlKeys).Assembly);
        configurator.AssemblyLoader.DirectAssemblies.Add(typeof(WebKeys).Assembly);
        configurator.AssemblyLoader.DirectAssemblies.Add(typeof(FeedKeys).Assembly);
        configurator.AssemblyLoader.DirectAssemblies.Add(typeof(CodeAnalysisKeys).Assembly);

        var assemblyFiles = build.PackageSpecs
            .SelectMany(x => x.Assemblies)
            .SelectMany(x => GlobFiles(NukeBuild.TemporaryDirectory / "_packages", x.TrimStart('/', '\\')))
            .Where(x => !x.Contains("Mocks"))
            .Distinct()
            .Select(x => GetRelativePath(NukeBuild.RootDirectory / "input", x));

        // if (!NukeBuild.IsLocalBuild || !GlobDirectories(NukeBuild.RootDirectory / "output/packages/*").Any())
        {
            Settings[DocsKeys.AssemblyFiles] = assemblyFiles;
        }

        Settings[DocsKeys.Title] = "Rocket Surgeons Guild";
        Settings[Keys.Host] = "rocketsurgeonsguild.com/";
        if (build.DeployToBranch == "master")
        {
            Settings[Keys.Host] = "rocketsurgeonsguild.github.io/";
        }
        else if (build.DeployToBranch == "docs")
        {
            Settings[Keys.Host] = "rocketsurgeonsguild.com/";
        }
        Settings[Keys.LinksUseHttps] = true;
        Settings[DocsKeys.Logo] = "/assets/img/logo.png";
        Settings[DocsKeys.IncludeDateInPostPath] = true;
        Settings[DocsKeys.IncludeGlobalNamespace] = false;
        Settings[DocsKeys.BaseEditUrl] = "https://github.com/RocketSurgeonsGuild/rocketsurgeonsguild.github.io/blob/dev/input/";

        Pipelines.InsertBefore(Docs.Code, "Package",
            new ReadFiles(NukeBuild.RootDirectory.ToString() + "/packages/*.yml"),
            new Yaml()
        );

        Pipelines.InsertAfter("Package", "PackageCategories",
            new GroupByMany((doc, _) => doc.List<string>("Categories"),
                new Documents("Package")
            ),
            new Meta(Keys.WritePath, (doc, _) => new FilePath("packages/" + doc.String(Keys.GroupKey).ToLower().Replace(" ", "-") + "/index.html")),
            new Meta(Keys.RelativeFilePath, (ctx, _) => ctx.FilePath(Keys.WritePath)),
            new OrderBy((ctx, _) => ctx.String(Keys.GroupKey))
        );

        Pipelines.Add("RenderPackage",
            new Documents("PackageCategories"),
            new Razor().WithLayout("/Shared/_PackageLayout.cshtml"),
            new WriteFiles()
        );

        Pipelines.InsertAfter(Docs.Api, "Conventions",
            new Documents(Docs.Api)
                .Where((doc, _) => doc.String(CodeAnalysisKeys.QualifiedName) == "Rocket.Surgery.Conventions.IConvention"),
            new SelectMany(doc => doc
                .DocumentList(CodeAnalysisKeys.ImplementingTypes)
                .Where(x => x.String(CodeAnalysisKeys.SpecificKind) == "Interface")
                .Where(x => !x.String(CodeAnalysisKeys.QualifiedName).StartsWith("Rocket.Surgery.Conventions"))
            ),
            new Meta("Children", (doc, _) =>
                doc
                    .DocumentList(CodeAnalysisKeys.ImplementingTypes)
                    .Where(x => x.String(CodeAnalysisKeys.SpecificKind) == "Class")
                    .OrderBy(x => x.String(CodeAnalysisKeys.Name))
            ),
            new Meta("ConventionName", (doc, _) =>
                doc.String(CodeAnalysisKeys.Name).Replace("Convention", "").TrimStart('I').Titleize()
            ),
            new Meta(Keys.WritePath, (doc, _) => new FilePath("conventions/" + doc.String("ConventionName").Camelize() + "/index.html")),
            new Meta(Keys.RelativeFilePath, (doc, _) => doc.FilePath(Keys.WritePath)),
            new OrderBy((ctx, _) => ctx.String("ConventionName"))
        );

        Pipelines.Add("RenderConventions",
            new Documents("Conventions"),
            new Razor().WithLayout("/Shared/_ConventonsLayout.cshtml"),
            new Headings(),
            new HtmlInsert("div#infobar-headings", (doc, ctx) => ctx.GenerateInfobarHeadings(doc)),
            new WriteFiles()
        );
    }
}

class SelectMany : IModule
{
    private readonly Func<IDocument, IEnumerable<IDocument>> _config;

    public SelectMany(Func<IDocument, IEnumerable<IDocument>> config)
    {
        _config = config;
    }

    public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
    {
        return inputs.SelectMany(_config);
    }
}
