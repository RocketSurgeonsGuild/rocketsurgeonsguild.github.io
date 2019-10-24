Title: Package References and Versions
Description: How are versions handled in a typical project
Order: 200
---

# Versioning is hard...
As Developers we have discovered that versioning is difficult even in the best of circumstances.  With the new project system available in .NET Core, this has gotten much easier beyond the days where you could have incorrect references to your packages directory if an upgrade or merge conflict went wrong.  Sending into hours (or days!) of NuGet pain and suffering.

# What we recommend
We recommend using [Microsoft.Build.CentralPackageVersions][CentralPackagingVersions].

This Sdk does a few things for you:
* Allows you to use a central `Packages.props` file with all of your versions defined
* Enforces versions are only defined in `Pacakges.props`.
* Adds support for `GlobalPackageReference` which can be used to apply common package references across all the projects in your solution, for example ensuring that Source Link is enabled on all projects.

This Sdk breaks a few things:
* After adding a package through the Visual Studio NuGet UI you might get some complaints or failures building.  This is due to the version being the project file, and not in `Packages.props`
  * You can override the version if you want to.
* This break also applies to `dotnet restore` or Visual Studio Code as well.

The one con is usually a deal breaker for a lot of people, but we really think this method is valuable because it isolates your version changes to one single location, which makes reviewing pull requests easier, and whenever you need to reference a package you don't have to go look up the version (if you manage the file directly).

NOTE: Also keep in mind we have additional tooling that plays off of this behavior, where we have our own MSBuild Sdk that allows you to easily include Rocket Surgery dependencies by a simple `PropertyGroup` change.

[CentralPackagingVersions]: https://www.nuget.org/packages/Microsoft.Build.CentralPackageVersions/ "Microsoft.Build.CentralPackageVersions"