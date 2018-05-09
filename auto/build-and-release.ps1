$Script:thisDir = Split-Path $MyInvocation.MyCommand.Path -Parent
& "${Script:thisDir}\build.ps1"
if ($?) { & "${Script:thisDir}\copy-fileset.ps1" }
