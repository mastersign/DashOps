$smallSizes = 16, 20, 24, 32
$largeSizes = 48, 64, 96, 128, 256

$root = $PSScriptRoot
$trgDir = "$root\palette-icons"

[xml]$iconSmall = Get-Content "$root\icon_small.svg"
[xml]$iconLarge = Get-Content "$root\icon_large.svg"

function setColors([xml]$svg, [string]$primary, [string]$secondary, [string]$tertiary, [string]$tertiaryAlpha) {
    $nsmgr = [System.Xml.XmlNamespaceManager]::new($svg.NameTable)
    $nsmgr.AddNamespace("svg", "http://www.w3.org/2000/svg")

    $style = $svg.DocumentElement.SelectSingleNode("//svg:style", $nsmgr)
    if (!$style) {
        Write-Error "Style node not found"
    }
    $css =
@"
    .primary-color { fill: $primary; }
    .secondary-color { fill: $secondary; }
    .tertiary-color { fill: $tertiary; opacity: $tertiaryAlpha; }
"@
    $style.InnerText = $css
}

$palette = Get-Content "$root\palette.json" | ConvertFrom-Json

foreach ($pc in $palette) {
    if (!$pc.Name) { continue }
    if ($pc.Name -ne "Pink") { continue }
    setColors $iconSmall $pc.Color $pc.Color $pc.Background "0.96"
    $smallSvgFile = "$trgDir\$($pc.Name)_icon_small.svg"
    $iconSmall.Save($smallSvgFile)
    setColors $iconLarge $pc.Color $pc.Color $pc.Background "0.90"
    $largeSvgFile = "$trgDir\$($pc.Name)_icon_large.svg"
    $iconLarge.Save($largeSvgFile)

    foreach ($s in $smallSizes) {
        inkscape --export-width=$s --export-filename="$trgDir\$($pc.Name)_icon_$s.png" $smallSvgFile
        # TODO sharpen with magic
    }
    foreach ($s in $largeSizes) {
        inkscape --export-width=$s --export-filename="$trgDir\$($pc.Name)_icon_$s.png" $largeSvgFile
    }
    # TODO combine PNGs into ICO
}
