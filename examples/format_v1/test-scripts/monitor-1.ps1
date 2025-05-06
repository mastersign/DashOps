param ([int]$chance)

$rand = New-Object System.Random
if ($rand.Next($chance) -eq 0)
{
    echo "I failed!"
    exit 1
}
else
{
    echo "I succeeded."
    exit 0
}
