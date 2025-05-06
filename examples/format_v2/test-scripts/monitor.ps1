param ([int]$chance)
Write-Output "Hi, it's me. The Monitor."
Write-Output "I fail in one of $chance attempts."
Write-Output "My exit code is irrelevant."

$rand = New-Object System.Random
if ($rand.Next($chance) -eq 0)
{
    Write-Output "Some text with the code word SUCCESSSS :-)"
}
else
{
    Write-Output "Some text without the required code word."
}

exit 1
