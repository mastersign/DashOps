param (
    [switch]$NoBuild,
    [switch]$NoSetupPackages,
    [switch]$NoMsiValidation,
    [switch]$NoSign,
    [string]$Configuration = "Release",
    [string]$Verbosity = "minimal"
)

$ErrorActionPreference = 'Stop'

$locales = @(
    "en-US",
    "de-DE"
)
$platforms = @(
    "x64"
    "x86"
    "AnyCPU"
)
$zipPlatforms = @(
    "AnyCPU"
)
$setupPlatforms = @(
    "x64"
    "x86"
)
$suppressedPlatformSuffix = "AnyCPU"
$excludeFiles = @(
    "*.deps.json"
    "*.pdb"
)
$keepSubDirs = @(
    "de"
)
$framework = "net48"
$verbosity = $Verbosity
$appName = "DashOps"
$appProject = "$PSScriptRoot\src\$appName\$appName.csproj"
$setupProject = "$PSScriptRoot\src\Setup\Setup.wixproj"
$setupPackageFile = "$PSScriptRoot\src\Setup\Package.wxs"

$version = ([Version]([xml](Get-Content $setupPackageFile)).Wix.Package.Version).ToString(3)

if (-not $version -match "\d+\.\d+\.\d+") {
    Write-Error "Failed to read project version from $setupProject"
}

if (!(Test-Path "$PSScriptRoot\release")) {
    mkdir "$PSScriptRoot\release" | Out-Null
}

$setupPackages = @()

foreach ($platform in $platforms) {
    if (!$NoBuild) {
        Write-Output ""
        Write-Output "Cleaning App Project ($platform) ..."
        dotnet clean $appProject --nologo -v $verbosity -f $framework -c $Configuration
        Write-Output ""
        Write-Output "Building App Project ($platform) ..."
        dotnet build $appProject --nologo -v $verbosity -f $framework -c $Configuration -p:Platform=$platform
        Write-Output ""
        Write-Output "Publishing App Project ($platform) ..."
        dotnet publish $appProject --nologo -v $verbosity -f $framework -c $Configuration -p:Platform=$platform -p:PublishProfile=$platform
    }

    $pubDir = "$PSScriptRoot\publish\$platform"
    foreach ($pattern in $excludeFiles) {
        Get-ChildItem "$pubDir\$pattern" | Remove-Item
    }
    if ($platform -ne "AnyCPU") {
        Get-ChildItem "$pubDir\*.runtimeconfig.json" | Remove-Item
    }
    foreach ($dir in (Get-ChildItem "$pubDir" -Directory)) {
        $keep = $False
        foreach ($subPath in $keepSubDirs) {
            if ("$pubDir\$subPath" -eq $dir.FullName) {
                $keep = $True
                break
            }
        }
        if (!$keep) {
            $dir | Remove-Item -Recurse
        }
    }
}
foreach ($platform in $zipPlatforms) {
    $pubDir = "$PSScriptRoot\publish\$platform"
    $suffix = if ($suppressedPlatformSuffix -eq $platform) { "" } else { "_$platform"}
    $zipArchive = "$PSScriptRoot\release\${appName}_v${version}${suffix}.zip"
    Compress-Archive -Path "$pubDir\*" -DestinationPath $zipArchive -CompressionLevel Optimal -Force
}

if (!$NoSetupPackages) {
    foreach ($platform in $setupPlatforms) {
        dotnet restore $setupProject --nologo -v $verbosity -p:Platform=$platform
        if ($NoMsiValidation) {
            dotnet build $setupProject --nologo -v $verbosity -c $Configuration -p:Platform=$platform --no-restore -p:SuppressValidation=true
        } else {
            dotnet build $setupProject --nologo -v $verbosity -c $Configuration -p:Platform=$platform --no-restore
        }

        $suffix = if ($suppressedPlatformSuffix -eq $platform) { "" } else { "_$platform"}

        foreach ($locale in $locales) {
            $setupPackage = if ($locales.Count -gt 1) {
                "$PSScriptRoot\release\${appName}_v${version}${suffix}_${locale}.msi"
            } else {
                "$PSScriptRoot\release\${appName}_v${version}${suffix}.msi"
            }
            Copy-Item "$PSScriptRoot\release\$platform\$locale\Setup.msi" $setupPackage -Force
            $setupPackages += $setupPackage
        }

        Remove-Item -Path "$PSScriptRoot\release\$platform" -Recurse
    }

    if (!$NoSign) {
        & "$PSScriptRoot\sign.ps1" $setupPackages
    }
}
