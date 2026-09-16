$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$projects = Get-ChildItem -Path $root -Recurse -Filter *.csproj | Sort-Object FullName

if ($projects.Count -eq 0) {
    Write-Host "No se encontraron proyectos .csproj."
    exit 0
}

foreach ($project in $projects) {
    Write-Host "Building $($project.FullName)"
    dotnet build $project.FullName --configuration Release
}

Write-Host "Build finalizado. Proyectos compilados: $($projects.Count)"
