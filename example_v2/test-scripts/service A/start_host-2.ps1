Write-Output "Script File: $($MyInvocation.MyCommand.Path)"
Write-Output "Script Args: $args"
Write-Output "Environment ENV_A: ${env:ENV_A}"
Write-Output "Environment ENV_B: ${env:ENV_B}"