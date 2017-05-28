Param(
    [string]$rootPath,
    [string]$SolutionFile = 'EESLP-DockerBITSOnly.sln'
)

$scriptPath = Split-Path $script:MyInvocation.MyCommand.Path

if([string]::IsNullOrEmpty($rootPath)) {
    $rootPath = "$scriptPath\.."
}

$SolutionFilePath = [IO.Path]::Combine($rootPath, $SolutionFile)

Write-Host "Using Solution file '$SolutionFile' at Path $SolutionFilePath" -ForegroundColor Yellow

# restore files
dotnet restore $SolutionFilePath

# publish files to specific docker obj output
dotnet publish $SolutionFilePath -c Release -o .\obj\Docker\publish