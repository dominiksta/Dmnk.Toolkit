#:sdk Cake.Sdk@6.0.0

var target = Argument("target", "build");
var configuration = Argument("configuration", "Release");

// ----------------------------------------------------------------------
// Tasks
// ----------------------------------------------------------------------

Task("build")
    .Does(() =>
    {
        DotNetBuild("./src/Dmnk.Toolkit.slnx", new DotNetBuildSettings
        {
            Configuration = configuration,
        });
    });

Task("test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        DotNetTest("./src/Dmnk.Toolkit.slnx", new DotNetTestSettings
        {
            Configuration = configuration,
            NoBuild = true,
        });
    });

Task("docs:clean")
    .Does(() =>
    {
        RimRaf("./docs/api");
        RimRaf("./docs/_site");
    });

Task("docs:build")
    .IsDependentOn("build")
    .IsDependentOn("docs:clean")
    .Does(() =>
    {
        StartProcess("dotnet", new ProcessSettings {
            Arguments = "docfx docfx.json"
        });
    });

Task("docs:serve")
    .IsDependentOn("docs:build")
    .Does(() =>
    {
        StartProcess("dotnet", new ProcessSettings {
            Arguments = "docfx serve",
            WorkingDirectory = "docs/_site"
        });
    });

Task("pack")
    .Does(() =>
    {
        StartProcess("dotnet", new ProcessSettings {
            Arguments = "pack src/Dmnk.Toolkit.slnx -c " + configuration + " -o ./artifacts"
        });
    });

// ----------------------------------------------------------------------
// Helpers
// ----------------------------------------------------------------------

void RimRaf(string path) {
    var rimraf = new DeleteDirectorySettings {
        Recursive = true,
        Force = true
    };
    if (System.IO.Path.Exists(path)) DeleteDirectory(path, rimraf);
}

// ----------------------------------------------------------------------
// Execution
// ----------------------------------------------------------------------

RunTarget(target); 