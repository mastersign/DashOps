echo "Script File: $($MyInvocation.MyCommand.Path)"
echo "Script Args: $args"

$rand = New-Object System.Random
if ($rand.Next(2) -eq 1) {
    echo "Random Error!"
    exit 1
}