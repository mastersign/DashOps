param ([int]$chance)
Write-Output "Hi, it's me. ${env:ENV_MONITOR}."

$rand = New-Object System.Random
if ($rand.Next($chance) -eq 0)
{
    Write-Output "I failed!"
    exit 1
}
else
{
    Write-Output "I succeeded."
    exit 0
}
