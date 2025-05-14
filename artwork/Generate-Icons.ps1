
$ErrorActionPreference = 'Stop'

$root = $PSScriptRoot
$workDir = "$root\icons"
$appDir = "$PSScriptRoot\..\src\DashOps"
$setupDir = "$PSScriptRoot\..\src\Setup"
$iconDir = "$appDir\Icons"

if (!(Test-Path $workDir)) { mkdir $workDir | Out-Null }

$sizeGroups = Get-Content "$root\sizes.json" | ConvertFrom-Json
$palette = Get-Content "$root\palette.json" | ConvertFrom-Json
$appIconTheme = "Default_Light"

# The icon size for the window icons is a hot topic,
# because with 100% HighDPI setting, the icons in the taskbar are rendered with 24px,
# but Windows requests the 32px version from the application icon instead of the 24px version.
# By intentionally leaving the 32px version out, Windows grabs the next larger version (48px)
# and scales it down to 24px. The scaled-down 48px version still looks better then
# the scaled-down 32px version.
# By including the 48px version, the icon looks not too bad with HighDPI > 100%.
#
# https://stackoverflow.com/questions/76928082/which-windows-api-returns-the-taskbar-icon-size
# https://stackoverflow.com/questions/32762463/windows-10-icons-in-taskbar-have-wrong-size
# https://www.reddit.com/r/Windows11/comments/ot4wts/return_the_option_with_resizing_the_taskbar/
$windowIcoSizes = 16, 24, 48

# Sizes used directly in the application as PNGs
$appImageSizes = 16, 32, 64

function setColors(
    [xml]$svg,
    [string]$primary,
    [string]$secondary,
    [string]$tertiary,
    [string]$tertiaryAlpha
) {
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

function renderInColor($name, $c1, $c2, $c3) {
    $isAppTheme = $name -eq $appIconTheme
    $newPNGs = $False
    foreach ($sg in $sizeGroups) {
        foreach ($s in $sg.Sizes) {
            if (!$isAppTheme -and !($s -in $appImageSizes) -and !($s -in $windowIcoSizes)) {
                continue
            }

            $svgFile = "$workDir\${name}_${s}.svg"
            $pngFile = "$workDir\${name}_${s}.png"
            if (Test-Path $pngFile) { continue }
            Write-Output "Rendering PNG in size ${s}px and color $name"
            $newPNGs = $true
            [double]$a3 = 1.0 - ([Math]::Sqrt($s) / 150)
            $a3 = [Math]::Round($a3 * 1000) / 1000
            setColors $sg.SVG $c1 $c2 $c3 $a3
            $sg.SVG.Save($svgFile)
            inkscape --export-width=$s --export-filename="$pngFile" $svgFile
            Remove-Item $svgFile
            magick convert $pngFile -sharpen 0x0.5 $pngFile
        }
    }

    if ($isAppTheme) {
        Write-Output "Combining PNGs into Application ICO"
        $appIcoFile = "$workDir\app.ico"
        magick convert "$workDir\${name}_*.png" $appIcoFile
    }

    $icoFile = "$workDir\$name.ico"
    if (!$newPNGs -and (Test-Path $icoFile)) { continue }
    Write-Output "Combining PNGs into Window ICO for color $name"
    $inputFiles =  $windowIcoSizes | ForEach-Object { "$workDir\${name}_${_}.png" }
    magick convert @inputFiles $icoFile
}

foreach ($sg in $sizeGroups) {
    Write-Output "Loading SVG for base size $($sg.Base)px"
    [xml]$svg = Get-Content "$root\icon_$($sg.Base).svg"
    Add-Member -InputObject $sg -Name "SVG" -MemberType NoteProperty -Value $svg
}

foreach ($pc in $palette) {
    if (!$pc.Name) { continue }

    $c1 = if ($pc.SymbolColor) { $pc.SymbolColor } else { "#FF0000" }
    $c2 = if ($pc.FrameColor) { $pc.FrameColor } else { $c1 }
    $dark = if ($pc.Dark) { $pc.Dark } else { "#000000" }
    $light = if ($pc.Light) { $pc.Light } else { "#FFFFFF" }

    renderInColor "$($pc.Name)_Light" $c1 $c2 $light
    renderInColor "$($pc.Name)_Dark" $c1 $c2 $dark
}

Copy-Item "$workDir\app.ico" "$appDir\icon.ico"
Copy-Item "$workDir\app.ico" "$setupDir\icon.ico"
foreach ($pc in $palette) {
    foreach ($theme in @("Light", "Dark")) {
        $name = "$($pc.Name)_${theme}"
        Copy-Item "$workDir\${name}.ico" "$iconDir\${name}.ico"
        foreach ($s in $appImageSizes) {
            Copy-Item "$workDir\${name}_${s}.png" "$iconDir\${name}_${s}.png"
        }
    }
}
