Write-Output "Script File: $($MyInvocation.MyCommand.Path)"
Write-Output "Script Args: $args"

$rand = New-Object System.Random
if ($rand.Next(2) -eq 1) {
    Write-Output "Random Error!"
    exit 1
}