#tool "nuget:https://api.nuget.org/v3/index.json?package=Wyam&version=1.2.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Wyam&version=1.2.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Yaml&version=2.0.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=YamlDotNet&version=4.2.1"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

class ProjectSpec
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

List<ProjectSpec> projectSpecs = new List<ProjectSpec>();

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

Task("Preview")
    .Does(() =>
    {
        Wyam(new WyamSettings
        {
            Recipe = "Docs",
            Theme = "Samson",
            UpdatePackages = true,
            Preview = true,
            Watch = true
        });
    });

Task("Default")
    .IsDependentOn("Preview");

RunTarget(target);