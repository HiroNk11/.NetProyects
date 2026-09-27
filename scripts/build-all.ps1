param([switch]$FrameworkOnly)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$projects = Get-ChildItem -Path $root -Recurse -Filter *.csproj | Sort-Object FullName
if ($FrameworkOnly) {
    $projects = @($projects | Where-Object {
        [xml]$projectXml = Get-Content -LiteralPath $_.FullName -Raw
        $projectXml.Project.PropertyGroup.TargetFramework -match '^net4'
    })
}

if ($projects.Count -eq 0) {
    Write-Host "No se encontraron proyectos .csproj."
    exit 0
}

foreach ($project in $projects) {
    Write-Host "Building $($project.FullName)"
    dotnet build $project.FullName --configuration Release
    if ($LASTEXITCODE -ne 0) {
        throw "Fallo la compilacion de $($project.FullName)"
    }
}

Write-Host "Build finalizado. Proyectos compilados: $($projects.Count)"
