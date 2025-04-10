param ([int]$chance, [int]$no)
Write-Output "Hi, it's me. ${env:ENV_MONITOR}."
Write-Output "I go by the number of $no."
Write-Output "I fail in one of $chance attempts."

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
