$MyDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

$sdkSetupUrl = "https://go.microsoft.com/fwlink/p/?linkid=870807"

$tmpDir = "$MyDir\..\tmp"
if (!(Test-Path $tmpDir))
{
    $_ = mkdir $tmpDir
}

Invoke-WebRequest $sdkSetupUrl -OutFile "$tmpDir\winsdksetup.exe"

$sdkSetup = Resolve-Path "$tmpDir\winsdksetup.exe"

& $sdkSetup /ceip off /uninstall
