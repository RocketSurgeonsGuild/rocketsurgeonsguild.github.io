Title: MSBuild Sdks
Order: 300
---

# What is an SDK?
MSBuild has added support for defining [custom SDKs][SdkDocs] that allow you to include additional build functionality when you're building your applications.

# Our custom SDKs

We currently have one SDK that we ship today, with potentially more to come.

## [Rocket.Surgery.Meta.Packages]
This package allows you to inside of a project automatically include all the Rocket Surgery assemblies you need based on your needs.

The following examples are examples of this in action.  We understand if you choose not use this functionality, because it does add a certain level of automagic behavior.  However it does allow you to emphasize dependencies that are specific to the project, and avoid confusing them with the "plumbing" dependencies.

# Properties

* `IncludeRocketSurgeryAbstractions`: Include the `PackageReference`s for just the abstractions (convention interfaces, etc.
* `IncludeRocketSurgeryAspNetCore`: Include the `PackageReference`s for working with AspNetCore 3+/
* `IncludeRocketSurgery`: Include the `PackageReference`s for the base extensions.
* `IncludeRocketSurgeryFunctions`: Include the required assemblies working with Azure Functions.
* `IncludeRocketSurgeryHosting`: Include the required assemblies for working with a project that uses the new Hosting Abstraction layer.

## Options

* `EnableAutofac` (default: `false`): Enables [Autofac] support.
* `EnableAutoMapper` (default: `true`): Enables [AutoMapper] support.
* `EnableCommandLine` (default: `false`): Enables [CommandLine] support.
* `EnableConfiguration` (default: `true`): Enables [Configuration] support.
* `EnableDependencyInjection` (default: `true`): Enables [DependencyInjection] support.
* `EnableDiagnostics` (default: `true`): Enables Diagnostics which are little helpful commands or other features that can be used to get an idea of the behavior of your application.
* `EnableExtensions` (default: `true`): Enables Rocket Surgeon Extensions that adds some helpful methods, but are not required.
* `EnableFluentValidation` (default: `true`): Enables [FluentValidation] support.
* `EnableLogging` (default: `true`): Enables [Logging] support with Microsoft Extensions
* `EnableMediatR` (default: `true`): Enables [MediatR] support.
* `EnableNewtonsoftJson` (default: `true`): Enables [Newtonsoft.Json] support for Asp Net Core 3
* `EnableSerilog` (default: `true`): Enables [Serilog] support.
* `EnableWebJobs` (default: `false`): Enables [WebJobs] (Azure Functions) support.


# Examples

## Asp Net Core 3 Example

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
    <Sdk Name="Rocket.Surgery.Meta.Packages" Version="3.0.0" />
    <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <IncludeRocketSurgeryAspNetCore>true</IncludeRocketSurgeryAspNetCore>
    </PropertyGroup>
</Project>
```

This will passively add references for...
```xml
<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" />
<PackageReference Include="Rocket.Surgery.AspNetCore" />
<PackageReference Include="Rocket.Surgery.AspNetCore.FluentValidation" />
<PackageReference Include="Rocket.Surgery.AspNetCore.FluentValidation.MediatR" />
<PackageReference Include="Rocket.Surgery.AspNetCore.FluentValidation.NewtonsoftJson"  />
<PackageReference Include="Rocket.Surgery.AspNetCore.Serilog" />
<PackageReference Include="Rocket.Surgery.Conventions.Diagnostics" />
<PackageReference Include="Rocket.Surgery.Extensions.AutoMapper" />
<PackageReference Include="Rocket.Surgery.Extensions.MediatR" />
<PackageReference Include="Rocket.Surgery.Hosting" />

```

### Hosting Example

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
    <Sdk Name="Rocket.Surgery.Meta.Packages" Version="3.0.0" />
    <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <IncludeRocketSurgeryHosting>true</IncludeRocketSurgeryHosting>
    </PropertyGroup>
</Project>
```

This will passively add references for...
```xml
<PackageReference Include="Rocket.Surgery.Extensions.AutoMapper" />
<PackageReference Include="Rocket.Surgery.Extensions.FluentValidation"  />
<PackageReference Include="Rocket.Surgery.Extensions.FluentValidation.MediatR"  />
<PackageReference Include="Rocket.Surgery.Extensions.MediatR" />
<PackageReference Include="Rocket.Surgery.Hosting" />
<PackageReference Include="Rocket.Surgery.Hosting.Serilog" />

```

### Library Example

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
    <Sdk Name="Rocket.Surgery.Meta.Packages" Version="3.0.0" />
    <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <IncludeRocketSurgery>true</IncludeRocketSurgery>
    </PropertyGroup>
</Project>
```

This will passively add references for...
```xml
<PackageReference Include="Rocket.Surgery.Extensions.AutoMapper" />
<PackageReference Include="Rocket.Surgery.Extensions.Configuration" />
<PackageReference Include="Rocket.Surgery.Extensions.DependencyInjection" />
<PackageReference Include="Rocket.Surgery.Extensions.FluentValidation"  />
<PackageReference Include="Rocket.Surgery.Extensions.FluentValidation.MediatR"  />
<PackageReference Include="Rocket.Surgery.Extensions.Logging" />
<PackageReference Include="Rocket.Surgery.Extensions.MediatR" />
<PackageReference Include="Rocket.Surgery.Hosting" />
<PackageReference Include="Rocket.Surgery.Hosting.Serilog" />
```


### Include Example

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
    <Sdk Name="Rocket.Surgery.Meta.Packages" Version="3.0.0" />
    <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <IncludeRocketSurgery>true</IncludeRocketSurgery>
        <EnableCommandLine>true</EnableCommandLine>
    </PropertyGroup>
</Project>
```

This will passively add references for...
```xml
<PackageReference Include="Rocket.Surgery.Extensions.AutoMapper" />
<PackageReference Include="Rocket.Surgery.Extensions.Configuration" />
<PackageReference Include="Rocket.Surgery.Extensions.CommandLine" />
<PackageReference Include="Rocket.Surgery.Extensions.DependencyInjection" />
<PackageReference Include="Rocket.Surgery.Extensions.FluentValidation"  />
<PackageReference Include="Rocket.Surgery.Extensions.FluentValidation.MediatR"  />
<PackageReference Include="Rocket.Surgery.Extensions.Logging" />
<PackageReference Include="Rocket.Surgery.Extensions.MediatR" />
<PackageReference Include="Rocket.Surgery.Hosting" />
<PackageReference Include="Rocket.Surgery.Hosting.Serilog" />
```



### Include Example

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
    <Sdk Name="Rocket.Surgery.Meta.Packages" Version="3.0.0" />
    <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <IncludeRocketSurgery>true</IncludeRocketSurgery>
        <EnableMediatR>false</EnableMediatR>
    </PropertyGroup>
</Project>
```

This will passively add references for...
```xml
<PackageReference Include="Rocket.Surgery.Extensions.AutoMapper" />
<PackageReference Include="Rocket.Surgery.Extensions.Configuration" />
<PackageReference Include="Rocket.Surgery.Extensions.DependencyInjection" />
<PackageReference Include="Rocket.Surgery.Extensions.FluentValidation"  />
<PackageReference Include="Rocket.Surgery.Extensions.Logging" />
<PackageReference Include="Rocket.Surgery.Hosting" />
<PackageReference Include="Rocket.Surgery.Hosting.Serilog" />
```



[Autofac]: https://www.nuget.org/packages/Autofac/
[AutoMapper]: https://www.nuget.org/packages/AutoMapper/
[CommandLine]: https://www.nuget.org/packages/McMaster.Extensions.CommandLineUtils/
[Configuration]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
[DependencyInjection]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
[Logging]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/
[FluentValidation]: https://www.nuget.org/packages/FluentValidation/
[MediatR]: https://www.nuget.org/packages/MediatR/
[Newtonsoft.Json]: https://www.nuget.org/packages/Newtonsoft.Json/
[Serilog]: https://www.nuget.org/packages/Serilog/
[WebJobs]: https://www.nuget.org/packages/Microsoft.Azure.WebJobs/
[SdkDocs]: https://docs.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk?view=vs-2019
[Rocket.Surgery.Meta.Packages]: https://www.nuget.org/packages/Rocket.Surgery.Meta.Packages/