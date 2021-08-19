var target = Argument("target", "Build");

var buildId = EnvironmentVariable("GITHUB_RUN_NUMBER");

var @ref = EnvironmentVariable("GITHUB_REF");
const string prefix = "refs/tags/";
var tag = !string.IsNullOrEmpty(@ref) && @ref.StartsWith(prefix) ? @ref.Substring(prefix.Length) : null;

Task("Build")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = "NOREACTOR",
        MSBuildSettings = new DotNetCoreMSBuildSettings()
    };
    
    var settingsWithReactor = new DotNetCoreBuildSettings
    {
        Configuration = "REACTOR",
        MSBuildSettings = new DotNetCoreMSBuildSettings()
    };

    if (tag != null) 
    {
        settings.MSBuildSettings.Properties["Version"] = settingsWithReactor.MSBuildSettings.Properties["Version"] = new[] { tag };
    }
    else if (buildId != null)
    {
        settings.VersionSuffix = settingsWithReactor.VersionSuffix = "ci." + buildId; "ci." + buildId;
    }

    foreach (var gamePlatform in new[] { "Steam", "Itch" })
    {
        settings.MSBuildSettings.Properties["GamePlatform"] = settingsWithReactor.MSBuildSettings.Properties["GamePlatform"] = new[] { gamePlatform };
        DotNetCoreBuild(".", settings);
    }
});

RunTarget(target);
