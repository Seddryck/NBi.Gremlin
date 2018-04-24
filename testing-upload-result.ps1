param(
    [parameter(Mandatory=$true)]
    [string]$target
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition)

Write-Host "Replacing the name of the tests"

$content = (Get-Content $("result.$target.xml") -Encoding UTF8) 
$content = $content -replace 'NBi.Testing.Core.Gremlin', $("NBi.Testing.$target.Core.Gremlin")
$content | Out-File $("result.$target.final.xml") -Encoding UTF8
$file = $("result.$target.final.xml")

Write-Host "Replacement executed in $file."

Write-Host "Uploading test results for job $env:APPVEYOR_JOB_ID"

$wc = New-Object 'System.Net.WebClient'
$wc.UploadFile("https://ci.appveyor.com/api/testresults/nunit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path .\$file))

Write-Host "Upload executed."