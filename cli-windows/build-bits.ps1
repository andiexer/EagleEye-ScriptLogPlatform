Param(
    [string]$rootPath,
    [string]$SolutionFile = 'EESLP-DockerBITSOnly.sln',
	[switch]$RemoveDockerImages
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

if($RemoveDockerImages) {
	./remove-dockerimages.ps1
}
<#
# build angular frontend
Write-Host "Build angular 2 frontend"
$path = pwd
cd ..\src\Frontend\UI
ng build --prod --aot
cd $path
#>