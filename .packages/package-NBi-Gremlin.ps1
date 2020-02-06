param(
    [parameter(Mandatory=$true)]
    [string]$version
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition)

Write-Host "Static set of dependencies ..."

$dependencies = @{}
$dependencies.add("Gremlin.Net", "<dependency id=`"Gremlin.Net`" version=`"3.3.2`" />")
$dependencies.add("NBi.Framework", "<dependency id=`"NBi.Framework`" version=`"[1.22.0-beta0055, 2.0)`" />")

Write-Host "Found $($dependencies.Count) dependencies ..."
$depList = $dependencies.Values -join [Environment]::NewLine + "`t`t"

# Download the icon if missing
$iconDirectory = "$root\NBi.Gremlin\images"
new-item -Path $iconDirectory -ItemType directory -force

$iconFile = "$iconDirectory\logo-2x.png"
if (Test-Path $iconFile)
{
    Write-Host "Icon already available."
} else {
	$urlIcon ="https://raw.githubusercontent.com/Seddryck/nbi/gh-pages/img/logo-2x.png"
    Write-Host "Downloading icon from $urlIcon ..."
    Invoke-WebRequest $urlIcon -OutFile $iconFile
    Write-Host "Icon downloaded."
}

#Set the current date for generating correct dates in copyright
$thisYear = get-date -Format yyyy
Write-Host "Setting copyright until $thisYear"

# check the version of Nuget to decide if it's supporting icon or iconUrl
$nugetVersion = (((nuget help | select -First 1).Split(':')) | select -Last 1).Trim()
Write-Host "Nuget's version: $nugetVersion"
if ($nugetVersion -lt '5.3')
{
    $xpath = ('/package/metadata/icon')
}
else
{
    $xpath = ('/package/metadata/iconUrl')
}

#For NBi.Gremlin (dll)
$lib = "$root\NBi.Gremlin\lib"
If (Test-Path $lib)
{
	Remove-Item $lib -recurse
}

new-item -Path $lib\net461\ -ItemType directory -force
new-item -Path $root\..\.nupkg -ItemType directory -force
Copy-Item $root\..\NBi.Core.Gremlin\bin\Debug\NBi.*Gremlin*.dll $lib\net461\

Write-Host "Setting .nuspec version tag to $version"

$content = (Get-Content $root\NBi.Gremlin.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$version
$content = $content -replace '\$thisYear\$',$thisYear
$content = $content -replace '\$depList\$',$depList

$xml = New-Object -TypeName System.Xml.XmlDocument
$xml.LoadXml($content)
$iconNode = $xml.SelectSingleNode($xpath)
$iconNode.ParentNode.RemoveChild($iconNode)

$xml.OuterXml | Out-File $root\NBi.Gremlin\NBi.Gremlin.compiled.nuspec -Encoding UTF8

& NuGet.exe pack $root\..\.packages\NBi.Gremlin\NBi.Gremlin.compiled.nuspec -Version $version -OutputDirectory $root\..\.nupkg
Write-Host "Package for NBi.Gremlin is ready"