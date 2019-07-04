# Troubleshooting IIS

## IIS does not respond

The issue is identified by lack of log output from log4net (or any other type of logging library) + the logs in the Event Viewer is also very obscure.

Try to change the PropertyGroup in the csproj file for the .net core project from

```xml
  <PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>
  </PropertyGroup>
```

to

```xml
  <PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <AspNetCoreHostingModel>OutOfProcess</AspNetCoreHostingModel>
    <AspNetCoreModuleName>AspNetCoreModule</AspNetCoreModuleName>
 </PropertyGroup>
```

Ref: https://stackoverflow.com/questions/53811569/using-netcore-2-2-and-using-the-in-process-hosting-model

or to

```xml
  <PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>
 </PropertyGroup>
```
