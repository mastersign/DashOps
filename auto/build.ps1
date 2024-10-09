param (
    $Mode = "Release",
    $MsBuildVerbosity = "minimal"
)

$thisDir = Split-Path $MyInvocation.MyCommand.Path -Parent
$rootDir = Split-Path $thisDir -Parent
$sourceDir = "$rootDir\src"
$solutionName = "DashOps"
$mode = $Mode
$target = "Clean;Restore;Build"
$verbosity = $MsBuildVerbosity
$solutionFile = "${solutionName}.sln" # relative to source dir
$msbuildPaths = @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
)
$msbuild = $msbuildPaths | Where-Object { Test-Path $_ } | Select-Object -First 1
Push-Location "$sourceDir"

# Build Solution
& $msbuild $solutionFile /v:$verbosity /t:$target /p:Configuration=$mode /m /nodereuse:false
$buildError = $LastExitCode

Pop-Location

exit $buildError
