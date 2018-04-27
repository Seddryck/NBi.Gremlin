param(
    [parameter(Mandatory=$true)]
    [string]$version
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition)

Write-Host "Calculating dependencies ..."

$dependencies = @{}
$solutionRoot = Join-Path ($root) ".."
Write-Host "Looking for projects in $solutionRoot ..."
$projects = Get-ChildItem $solutionRoot | ?{ $_.PSIsContainer -and $_.Name -like "*Gremlin*"-and $_.Name -notLike "*Testing*"} | select Name, FullName
foreach($proj in $projects)
{
    $projName = $proj.name
    Write-Host "Looking for dependencies in project $projName ..."
    $path = Join-Path ($proj.FullName) "packages.config"
        
    if(Test-Path $path)
    {
        [xml]$packages = Get-Content $path
        foreach($package in $packages.FirstChild.NextSibling.ChildNodes)
        {
            if (!$dependencies.ContainsKey($package.id)) {$dependencies.add($package.id, "<dependency id=""$($package.id)"" version=""$(($package.allowedVersions, $package.version -ne $null)[0])"" />")}
        }
    }
}

Write-Host "Found $($dependencies.Count) dependencies ..."
$depList = $dependencies.Values -join [Environment]::NewLine + "`t`t"

#For NBi.Gremlin (dll)
$lib = "$root\NBi.Gremlin\lib\net46\"
If (Test-Path $lib)
{
	Remove-Item $root\NBi.Gremlin\lib -recurse
}
new-item -Path $lib -ItemType directory
new-item -Path $root\..\.nupkg -ItemType directory -force
Copy-Item $root\..\NBi.Core.Gremlin\bin\Debug\NBi.*Gremlin*.dll $lib

Write-Host "Setting .nuspec version tag to $version"

$content = (Get-Content $root\NBi.Gremlin\NBi.Gremlin.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$version
$content = $content -replace '\$depList\$',$depList

$content | Out-File $root\NBi.Gremlin\NBi.Gremlin.compiled.nuspec -Encoding UTF8

& NuGet.exe pack $root\..\.packages\NBi.Gremlin\NBi.Gremlin.compiled.nuspec -Version $version -OutputDirectory $root\..\.nupkg
