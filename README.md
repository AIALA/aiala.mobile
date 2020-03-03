# aiala.mobile
Mobile app for AIALA

## Project structure
AIALA mobile is intended to be a cross-plattform app to use on _iOS_ and _Android_ devices. Along the concepts of xamarin there is a common/shared codebase and where required device specific implementations.

## How to build
AIALA mobile is build against .NET Standard 2.0 and is using several 3rd party libraries and components.
* Download and Install [Visual Studio-Tools for Xamarin](https://visualstudio.microsoft.com/xamarin)
* Ensure [NuGet](https://www.nuget.org/) is installed and configured 
* Connect to xappido Portal package feed ([ask for access and license](mailto:aiala@xappido.com))

To build _iOS_ app a configured Mac must be connected to visual stuido.

To build _Andoid_ app a windows computer with all installed tools, libraries and sdk for Android development is required.

On the other hand, [Azure DevOps](https://azure.microsoft.com/en-us/services/devops/) allows to configure a pipeline to build _iOS_ and _Android_ apps online. Furthermore, [Visual Studio App Center](https://azure.microsoft.com/en-us/services/app-center/) provides integrated developer services for building, testing, releasing, and monitoring mobile apps.

## Package feed
Configure package feeds (especially xappido Portal package feed) either globally or locally within aiala.backend.

Add xappido feed globally by using `nuget cli` and authorize by using credential provider

`nuget sources Add -Name "xappido" -Source https://xappido.pkgs.visualstudio.com/...`

If your prefer configure feed locally, use Visual Studios' NuGet Package Manager or add a nuget.config file to your project, in the same folder as .sln file.

```XML
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="xappido" value="https://xappido.pkgs.visualstudio.com/..." />
  </packageSources>
</configuration>
```

## Configuration
All required options to connect _backend_ and _token server_ are stored within file `configuration/ApplicationSettingsPreset.cs` as presets.

While starting app, it will be possible to change those values.

## Documentation
See mentioned documentation for any further information. If you like to use AIALA within your organisation feel free and get in touch with [AIALA Project Team](mailto:aiala@xappido.com).