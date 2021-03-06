﻿Param(
    [string]$rootPath,
    [string]$SolutionFile = 'EESLP-DockerBITSOnly.sln',
    [switch]$RemoveDockerImages,
    [switch]$WithoutFrontend
)

$scriptPath = Split-Path $script:MyInvocation.MyCommand.Path

if ([string]::IsNullOrEmpty($rootPath)) {
    $rootPath = "$scriptPath\.."
}

$SolutionFilePath = [IO.Path]::Combine($rootPath, $SolutionFile)

Write-Host "Using Solution file '$SolutionFile' at Path $SolutionFilePath" -ForegroundColor Yellow

dotnet clean $SolutionFile -c Release -o .\obj\Docker\publish

# restore files
dotnet restore $SolutionFilePath

# publish files to specific docker obj output
dotnet publish $SolutionFilePath -c Release -o .\obj\Docker\publish

if ($RemoveDockerImages) {
    ./remove-dockerimages.ps1
}

# build angular frontend
if (!($WithoutFrontend)) {
    Write-Host "Build angular 2 frontend"
    $path = pwd
    cd $rootPath\src\Frontend\UI
    npm install
    ng build --prod --aot
    cd $path
}