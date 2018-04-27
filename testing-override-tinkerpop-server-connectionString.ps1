$root = (split-path -parent $MyInvocation.MyCommand.Definition)
if ($env:APPVEYOR)
{
    Write-Host "Replacing the content of the *.user.config file by *.tinkerpop.config"

    $content = (Get-Content $root\NBi.Testing.Core.Gremlin\ConnectionString.tinkerpop.config -Encoding UTF8) 
    $content = $content -replace '\$AzureAuthKey\$',$azureGremlinConnectionString
    $content | Out-File $root\NBi.Testing.Core.Gremlin\bin\Debug\ConnectionString.user.config -Encoding UTF8

    Write-Host "Replacement executed."
}
else
{
    Write-Host "Not running on an appVeyor environment, skipping the override of the *.user.config file."
}
