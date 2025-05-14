param (
    $Configuration = "Release",
    $Verbosity = "minimal"
)

$rootDir = Resolve-Path "$PSScriptRoot\.."
$sourceDir = "$rootDir\src"
$projectFile = "Setup\Setup.wixproj" # relative to source dir

Push-Location "$sourceDir"

# Build Solution
dotnet build $projectFile -v $Verbosity -c $Configuration
$buildError = $LastExitCode

Pop-Location

exit $buildError
