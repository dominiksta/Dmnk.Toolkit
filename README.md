# Dmnk.Toolkit

[//]: # (This file is shown as the README on GitHub)
[//]: # (Do not use DocFX syntax here, as it will be rendered by GitHub and not DocFX)

Mostly small Blazor-related C# libraries. [Documentation](https://dominiksta.github.io/Dmnk.Toolkit/) and Code 
are Work-In-Progress.

## Contributing & Maintenance

Note that while the `Dmnk.Blazor.InteractiveTests.*` packages only work on Windows, they should 
absolutely build and cause no trouble working on the rest of the codebase on Linux or Mac.

- Run `dotnet cake.cs --target=pack --configuration=Release` to build the NuGet packages.
- Run `dotnet cake.cs --target=test` to run the tests.
- The documentation is built from the code and published via GitHub pages.
  - See [publish-docs.yml](.github/workflows/docs_pages.yml)
  - Run `dotnet cake.cs --target=docs:serve` to preview the docs locally.
- New releases are published to nuget.org via a [GitHub action](.github/workflows/publish_nuget.yml) 
  that runs when a new release is published. Before publishing a release, ensure that:
  - The CHANGELOG.md file is updated accordingly
  - The AnalyzerReleases.(Shipped|Unshipped).md files of any analyzer are update accordingly.
  - The ReleaseNotes in the csproj files are updated accordingly
