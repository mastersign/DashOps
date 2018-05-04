param (
    $Mode = "Release",
    $MsBuildVerbosity = "minimal"
)

$thisDir = Split-Path $MyInvocation.MyCommand.Path -Parent
$rootDir = Split-Path $thisDir -Parent
$sourceDir = "$rootDir\src"
$solutionName = "DashOps"
$projectNames = @("DashOps")
$clrVersion = "4.0.30319"
$toolsVersion = "4.0"
$projectToolsVersion = "15.0"
$compilerPackageVersion = "2.7.0"
$compilerPackageFramework = "net46"
$langVersion = "7.2"
$mode = $Mode
$target = "Clean;Build"
$verbosity = $MsBuildVerbosity
$msbuild = "$env:SystemRoot\Microsoft.NET\Framework\v${clrVersion}\MSBuild.exe"
$solutionFile = "${solutionName}.sln" # relative to source dir

# Add Roslyn compiler to projects
& "$thisDir\prepare-project-compiler.ps1" `
    -Projects $projectNames `
    -ToolsVersion $projectToolsVersion `
    -CompilerPackageVersion $compilerPackageVersion `
    -CompilerPackageFramework $compilerPackageFramework `
    -LangVersion $langVersion

# Download NuGet
$nugetPath = "$thisDir\nuget.exe"
& "$thisDir\download-nuget.ps1" $nugetPath

pushd "$sourceDir"

# Restore NuGet packages
echo ""
echo "Restoring NuGet packages ..."
& $nugetPath restore $solutionFile
if ($LastExitCode -ne 0)
{
    Write-Error "Restoring NuGet packages failed."
    popd
    exit 1
}

# Build Solution
& $msbuild $solutionFile /v:$verbosity /tv:$toolsVersion /t:$target /p:Configuration=$mode /m /nodereuse:false
$buildError = $LastExitCode

popd

# Remove Roslyn compiler from projects
& "$thisDir\prepare-project-compiler.ps1" -Projects $projectNames -Remove

exit $buildError
