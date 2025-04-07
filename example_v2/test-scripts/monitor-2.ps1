param ([int]$chance)

$rand = New-Object System.Random
if ($rand.Next($chance) -eq 0)
{
    Write-Output "Some text with the code word SUCCESS :-)"
}
else
{
    Write-Output "Some text without the required code word."
}
