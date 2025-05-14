param (
    $Configuration = "Release"
)

$rootDir = Resolve-Path "$PSScriptRoot\.."
$binDir = Resolve-Path "$rootDir\src\DashOps\bin\$Configuration\net48"
$releaseDir = "$rootDir\release"

if (Test-Path $releaseDir) {
    Remove-Item "$releaseDir\*" -Recurse -Force
} else {
    mkdir $releaseDir | Out-Null
}

function CopyLocalization($locale) {
    mkdir "$releaseDir\$locale"
    Copy-Item "$binDir\$locale\*.dll" "$releaseDir\$locale"
}

Copy-Item "$rootDir\README.md" $releaseDir
Copy-Item "$binDir\*.exe" $releaseDir
Copy-Item "$binDir\*.exe.config" $releaseDir
Copy-Item "$binDir\*.dll" $releaseDir
CopyLocalization "de"
