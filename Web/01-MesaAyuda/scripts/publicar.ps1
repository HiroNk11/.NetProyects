$ErrorActionPreference = 'Stop'
$solutionRoot = Split-Path $PSScriptRoot -Parent
$artifacts = Join-Path $solutionRoot 'artifacts'
$publishDirectory = Join-Path $artifacts ('publish-' + [Guid]::NewGuid().ToString('N'))
$archive = Join-Path $artifacts 'mesa-ayuda.zip'
dotnet publish (Join-Path $solutionRoot 'src/MesaAyuda.Web/MesaAyuda.Web.csproj') -c Release -o $publishDirectory -p:DebugType=None -p:DebugSymbols=false
if ($LASTEXITCODE -ne 0) { throw 'Falló la publicación.' }
Compress-Archive -Path (Join-Path $publishDirectory '*') -DestinationPath $archive -Force
Write-Host "Paquete preparado: $archive"
