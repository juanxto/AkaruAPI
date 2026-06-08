# Roda todos os testes automatizados do projeto
$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot\..

Write-Host "=== Akaru API — Testes automatizados ===" -ForegroundColor Yellow
dotnet test --verbosity normal

if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "`nTodos os 16 testes passaram!" -ForegroundColor Green
