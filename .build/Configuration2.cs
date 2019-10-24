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
using System.IO;
using Wyam.Highlight;

class WyamConfiguration : ConfigurationEngineBase
{
    public WyamConfiguration(Engine engine, Build build) : base(engine)
    {
        var configurator = new Configurator(engine);
        configurator.Recipe = new Wyam.Docs.Docs();
        configurator.Theme = "Samson";
        configurator.Configure("");

        engine.ApplicationInput = NukeBuild.RootDirectory / "input";
        configurator.AssemblyLoader.DirectAssemblies.Add(typeof(HtmlKeys).Assembly);
        configurator.AssemblyLoader.DirectAssemblies.Add(typeof(WebKeys).Assembly);
        configurator.AssemblyLoader.DirectAssemblies.Add(typeof(FeedKeys).Assembly);
        configurator.AssemblyLoader.DirectAssemblies.Add(typeof(CodeAnalysisKeys).Assembly);
        configurator.AssemblyLoader.DirectAssemblies.Add(typeof(Highlight).Assembly);

        var assemblyFiles = build.PackageSpecs
                .SelectMany(x => x.Assemblies.Select(z => z.TrimStart('/', '\\')))
                .SelectMany(x => GlobFiles(NukeBuild.TemporaryDirectory / "_packages", x))
                .Where(x => !x.Contains("Mocks"))
                .Select(x => GetRelativePath(NukeBuild.RootDirectory / "input", x));

        // if (!NukeBuild.IsLocalBuild)
        // {
            Settings[DocsKeys.AssemblyFiles] = assemblyFiles.ToArray();
        // }
        // else
        // {
        //     Settings[DocsKeys.AssemblyFiles] = Array.Empty<string>();
        // }

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

        Pipelines.Add("RenderConventions",
            new Documents("Conventions"),
            new Razor().WithLayout("/Shared/_ConventionsLayout.cshtml"),
            new Headings(),
            new HtmlInsert("div#infobar-headings", (doc, ctx) => ctx.GenerateInfobarHeadings(doc)),
            new WriteFiles()
        );

        Pipelines.Add("RenderPackage",
            new Documents("PackageCategories"),
            new Razor().WithLayout("/Shared/_PackageLayout.cshtml"),
            new WriteFiles()
        );

        Pipelines.InsertAfter(Docs.Api, "AllConventions",
            new Documents(Docs.Api)
                .Where((doc, _) => doc.DocumentList(CodeAnalysisKeys.AllInterfaces)?.Any(d => d.String(CodeAnalysisKeys.QualifiedName) == "Rocket.Surgery.Conventions.IConvention") == true)
                .Where((doc, _) => !doc.String(CodeAnalysisKeys.QualifiedName).StartsWith("Rocket.Surgery.Conventions")),
            new Meta("ConventionName", (doc, _) =>
                doc.String(CodeAnalysisKeys.SpecificKind) == "Interface"
                ? doc.String(CodeAnalysisKeys.Name).Replace("Convention", "").TrimStart('I').Titleize()
                : doc.String(CodeAnalysisKeys.Name).Replace("Convention", "").Titleize()
            ),
            new Meta("Context", (doc, _) => doc
                .DocumentList(CodeAnalysisKeys.AllInterfaces)
                .First(x => x.String(CodeAnalysisKeys.Name) == "IConvention")
                .DocumentList(CodeAnalysisKeys.TypeArguments)
                .First()
            )
        );

        Pipelines.InsertAfter("AllConventions", "Conventions",
            new Documents("AllConventions")
                .Where((doc, _) => doc.String(CodeAnalysisKeys.SpecificKind) == "Interface"),
            new Meta("Children", (doc, e) => Documents.FromPipeline("AllConventions")
                .Where(x => x.String(CodeAnalysisKeys.SpecificKind) == "Class")
                .Where(x => x.DocumentList(CodeAnalysisKeys.AllInterfaces)?.Any(z => z.String(CodeAnalysisKeys.QualifiedName) == doc.String(CodeAnalysisKeys.QualifiedName)) == true)
                .OrderBy(x => x.String("ConventionName"))
            ),
            new Meta(Keys.WritePath, (doc, _) => new FilePath("conventions/" + doc.String("ConventionName").Camelize() + "/index.html")),
            new Meta(Keys.RelativeFilePath, (doc, _) => doc.FilePath(Keys.WritePath)),
            new OrderBy((ctx, _) => ctx.String("ConventionName"))
        );


    }
}

class FileNameComparer : EqualityComparer<string>
{
    public override bool Equals(string x, string y)
    {
        return Path.GetFileNameWithoutExtension(x) == Path.GetFileNameWithoutExtension(y);
    }

    public override int GetHashCode(string obj)
    {
        return obj == null ? 0 : Path.GetFileNameWithoutExtension(obj).GetHashCode();
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
