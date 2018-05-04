param ([int]$chance)

$rand = New-Object System.Random
if ($rand.Next($chance) -eq 0)
{
    echo "Some text with the code word SUCCESS :-)"
}
else
{
    echo "Some text without the required code word."
}
