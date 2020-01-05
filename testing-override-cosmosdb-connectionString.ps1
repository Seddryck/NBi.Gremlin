param(
    [parameter(Mandatory=$true)]
    [string]$azureGremlinConnectionString
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition)
if ($env:APPVEYOR)
{
    Write-Host "Overiding the *.user.config file with the *.cosmosdb.config file"

    $content = (Get-Content "$root\NBi.Testing.Core.Gremlin\ConnectionString.cosmosdb.config" -Encoding UTF8) 
    $content | Out-File "$root\NBi.Testing.Core.Gremlin\bin\Debug\ConnectionString.user.config" -Encoding UTF8

    Write-Host "Replacement executed."
}
else
{
    Write-Host "Not running on an appVeyor environment, skipping the override of the *.user.config file."
}
