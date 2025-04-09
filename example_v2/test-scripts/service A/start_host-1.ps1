Write-Output "Script File: $($MyInvocation.MyCommand.Path)"
Write-Output "Script Args: $args"
Write-Output "Environment ENV_A: ${env:ENV_A}"
Write-Output "Environment ENV_B: ${env:ENV_B}"

if (Get-Command myprog -ErrorAction SilentlyContinue) {
    myprog
} else {
    Write-Output "myprog is not on PATH"
}

Read-Host "Presse ENTER to continue"
