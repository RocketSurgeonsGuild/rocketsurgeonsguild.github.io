Description: Custom Sdk's
Order: 300
---

# What is an SDK?
MSBuild has added support for defining [custom SDKs][SdkDocs] that allow you to include additional build functionality when you're building your applications.

# Our custom SDKs

We currently have one SDK that we ship today, with potentially more to come.

## [Rocket.Surgery.Meta.Packages]
This package allows you to inside of a project automatically include all the Rocket Surgery assemblies you need based on your needs.

The following examples are examples of this in action.  We understand if you choose not use this functionality, because it does add a certain level of automagic behavior.  However it does allow you to emphasize dependencies that are specific to the project, and avoid confusing them with the "plumbing" dependencies.

### Asp.Net Core 3 Example

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
    <Sdk Name="Rocket.Surgery.Meta.Packages" Version="3.0.0" />
    <PropertyGroup>
        <TargetFramework>netcoreapp3.0</TargetFramework>
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
        <TargetFramework>netcoreapp3.0</TargetFramework>
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
        <TargetFramework>netcoreapp3.0</TargetFramework>
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
        <TargetFramework>netcoreapp3.0</TargetFramework>
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
        <TargetFramework>netcoreapp3.0</TargetFramework>
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





[SdkDocs]: https://docs.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk?view=vs-2019
[Rocket.Surgery.Meta.Packages]: https://www.nuget.org/packages/Rocket.Surgery.Meta.Packages/