param ($TargetPath = $("$(Split-Path $MyInvocation.MyCommand.Path -Parent)\nuget.exe"))

$nugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$nuget4Url = "https://dist.nuget.org/win-x86-commandline/v4.0.0/nuget.exe"


if (!(Test-Path $TargetPath))
{
    echo "Downloading latest NuGet.exe ..."
    $wc = New-Object System.Net.WebClient
    $wc.DownloadFile($nugetUrl, $TargetPath)
}
[Version]$nugetVersion = [Diagnostics.FileVersionInfo]::GetVersionInfo($TargetPath).FileVersion
if ($nugetVersion.Major -lt 4)
{
    echo "Latest NuGet is earlier then 4.x."
    del $TargetPath
    echo "Downloading NuGet.exe 4.0 ..."
    $wc = New-Object System.Net.WebClient
    $wc.DownloadFile($nuget4Url, $TargetPath)
}
