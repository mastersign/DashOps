param (
    $Name = "default",
    $FileSetsDir = $([IO.Path]::Combine((Split-Path $MyInvocation.MyCommand.Path -Parent), "filesets")),
    $BaseDir = $(Split-Path (Split-Path $MyInvocation.MyCommand.Path -Parent) -Parent),
    $TargetDir = $([IO.Path]::Combine($BaseDir, "release"))
)

echo "FileSet: $Name"
echo "FileSetsDir: $FileSetsDir"
echo "BaseDir: $BaseDir"
echo "TargetDir: $TargetDir"

if (!(Test-Path $TargetDir)) {
    mkdir $TargetDir | Out-Null
}
$Script:trgDir = $TargetDir

$Script:thisDir = Split-Path $MyInvocation.MyCommand.Path -Parent
$sets = @()
$files = @()

function resolve_name($fileSetsDir, $n) {
    return [IO.Path]::Combine($fileSetsDir, "$n.txt")
}

function copy_file($baseDir, $path) {
    if ($files -contains $path) { return }
    $files += $path
    $absPath = [IO.Path]::Combine($baseDir, $path)
    Copy-Item $absPath $Script:trgDir -Recurse -Force
}

function switch_to($subdir) {
    $path = [IO.Path]::Combine($TargetDir, $subdir)
    if (!(Test-Path $path)) {
        mkdir $path | Out-Null
    }
    $script:trgDir = $path
}

function switch_back() {
    $script:trgDir = $TargetDir
}

function process_set($fileSetsDir, $setName, $baseDir) {
    if ($sets -contains $setName) { return }
    $sets += $setName

    $path = resolve_name $fileSetsDir $setName
    if (![IO.File]::Exists($path)) {
        throw "FileSet '$setName' not found in '$path'."
    }
    $lines = Get-Content $path
    foreach ($line in $lines) {
        $line = $line.Trim()
        if (-not $line) { continue }
        if ($line.StartsWith("#include ")) {
            process_set $(Split-Path $path -Parent) $line.Substring(9) $baseDir
        } elseif ($line.StartsWith("#begin_dir ")) {
            switch_to $line.Substring(11)
        } elseif ($line -eq "#end_dir") {
            switch_back
        } else {
            copy_file $baseDir $line
        }
    }
}

process_set $FileSetsDir $Name $BaseDir
