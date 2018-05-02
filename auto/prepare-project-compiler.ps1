param (
  [switch]$Remove,
  $CompilerPackageId = "Microsoft.Net.Compilers",
  $CompilerPackageVersion = "2.8.0",
  $LangVersion = "7.2"
)

$thisDir = Split-Path $MyInvocation.MyCommand.Path -Parent
$sourceDir = Resolve-Path "$thisDir\..\src"

# https://github.com/dotnet/roslyn/wiki/NuGet-packages
$projects = @(
    @{
        "name" = "DashOps"
        "targetFramework" = "net47"
        "toolsVersion" = "15.0"
    }
)

function removeCompilerPackage($name, $packageId)
{
    $packagesConfigFile = "$sourceDir\$name\packages.config"
    if (Test-Path $packagesConfigFile -PathType Leaf)
    {
        [xml]$packagesConfig = Get-Content $packagesConfigFile
        $killNodes = @()
        foreach ($p in $packagesConfig.SelectNodes("/packages/package"))
        {
            if ($p.id -eq $packageId)
            {
                $killNodes += $p
            }
        }
        foreach ($n in $killNodes)
        {
            echo "Removing package $($n.id) v$($n.version) from project $name"
            $_ = $n.ParentNode.RemoveChild($n)
        }
        $pp = $packagesConfig.SelectNodes("/packages/package")
        if ($pp.Count -gt 0)
        {
            $packagesConfig.Save($packagesConfigFile)
        }
        else
        {
            del $packagesConfigFile
        }
    }
}

function addCompilerPackage($name, $packageId, $packageVersion, $framework)
{
    $packagesConfigFile = "$sourceDir\$name\packages.config"
    if (!(Test-Path $packagesConfigFile -PathType Leaf))
    {
        [xml]$newDoc = "<?xml version=`"1.0`" encoding=`"utf-8`"?>`n<packages></packages>"
        $newDoc.Save($packagesConfigFile)
    }
    echo "Adding package $packageId v$packageVersion to project $name"
    [xml]$packagesConfig = Get-Content $packagesConfigFile
    [Xml.XmlElement]$p = $packagesConfig.CreateElement("package")
    $p.SetAttribute("id", $packageId)
    $p.SetAttribute("version", $packageVersion)
    if ($framework) { $p.SetAttribute("targetFramework", $framework) }
    $p.SetAttribute("developmentDependency", "true")
    $pp = $packagesConfig.SelectSingleNode("/packages")
    $_ = $pp.AppendChild($p)
    $packagesConfig.Save($packagesConfigFile)
}

function prepareProject($name, $toolsVersion, $langVersion)
{
    $projectFile = "$sourceDir\$name\$name.csproj"
    if (!(Test-Path $projectFile -PathType Leaf))
    {
        Write-Warning "Could not find project file: $projectFile"
        return
    }
    [xml]$project = Get-Content $projectFile
    $nsMgr = New-Object System.Xml.XmlNamespaceManager($project.NameTable)
    $ns = "http://schemas.microsoft.com/developer/msbuild/2003"
    $nsMgr.AddNamespace("msb", $ns)

    $p = $project.SelectSingleNode("/msb:Project", $nsMgr)

    if ($toolsVersion)
    {
        echo "Setting tools version in $name to $toolsVersion"
        $p.SetAttribute("ToolsVersion", $toolsVersion)
    }

    echo "Setting compiler in $name to $CompilerPackageVersion"
    [Xml.XmlElement]$import = $null
    foreach ($i in $p.SelectNodes("msb:Import", $nsMgr))
    {
        if ($i.GetAttribute("Project") -like "*Microsoft.Net.Compilers*")
        {
            $import = $i
            break
        }
    }
    if (!$import)
    {
        [Xml.XmlElement]$import = $project.CreateElement("Import", $ns)
        $import = $p.PrependChild($import)
    }
    $packagePath = "..\packages\Microsoft.Net.Compilers.${CompilerPackageVersion}\build\Microsoft.Net.Compilers.props"
    $import.SetAttribute("Project", $packagePath)
    $import.SetAttribute("Condition", "Exists('${packagePath}')")

    echo "Setting language version in $name to $langVersion"
    foreach ($pt in $p.SelectNodes("msb:PropertyGroup/msb:PlatformTarget", $nsMgr))
    {
        [Xml.XmlElement]$pg = $pt.ParentNode
        [Xml.XmlElement]$lv = $pg.SelectSingleNode("msb:LangVersion", $nsMgr)
        if (!$lv)
        {
            $lv = $project.CreateElement("LangVersion", $ns)
            $pg.AppendChild($lv)
        }
        $lv.InnerText = $langVersion
    }

    $project.Save($projectFile)
}

foreach ($project in $projects)
{
    removeCompilerPackage $project.name $CompilerPackageId
    if (!$Remove)
    {
        addCompilerPackage $project.name $CompilerPackageId $CompilerPackageVersion $project.targetFramework
        prepareProject $project.name $project.toolsVersion $LangVersion
    }
}