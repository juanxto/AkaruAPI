# Testa a Akaru API localmente (fluxo completo do mobile)
# Uso: .\scripts\testar-api.ps1
# Requer: API rodando em http://localhost:5001 com UseMockAuth: true

$ErrorActionPreference = "Stop"
$BaseUrl = "http://localhost:5001"
$Token = "dev-test-token"
$Headers = @{ Authorization = "Bearer $Token" }

function Test-Step {
    param([string]$Nome, [scriptblock]$Acao)
    Write-Host ""
    Write-Host ">> $Nome" -ForegroundColor Cyan
    & $Acao
    Write-Host "   OK" -ForegroundColor Green
}

Write-Host "=== Akaru API - Teste do fluxo mobile ===" -ForegroundColor Yellow

try {
    $health = Invoke-RestMethod -Uri "$BaseUrl/health" -Method Get -TimeoutSec 5
    Write-Host "API online: $health"
} catch {
    Write-Host "API nao esta rodando em $BaseUrl" -ForegroundColor Red
    Write-Host "Execute em outro terminal: cd src\Akaru.API; dotnet run" -ForegroundColor Yellow
    exit 1
}

Test-Step "POST /api/usuarios/sync" {
    $script:usuario = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios/sync" -Method Post -Headers $Headers
    Write-Host "   Usuario id=$($usuario.id) email=$($usuario.email)"
}

Test-Step "GET /api/usuarios/me" {
    $perfil = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios/me" -Method Get -Headers $Headers
    Write-Host "   Nome=$($perfil.nome)"
}

Test-Step "PUT /api/usuarios/me" {
    $body = @{ nome = "Juan Pablo"; cidade = "Sao Paulo"; estado = "SP" } | ConvertTo-Json
    $atualizado = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios/me" -Method Put -Headers $Headers `
        -Body $body -ContentType "application/json"
    Write-Host "   Nome=$($atualizado.nome) Cidade=$($atualizado.cidade)"
}

Test-Step "POST /api/plantios" {
    $body = @{
        culturaId = 3
        latitude = -23.5505
        longitude = -46.6333
        dataPlantio = (Get-Date).ToUniversalTime().ToString("o")
        detalhes = "Solo argiloso"
        cidade = "Sao Paulo"
        estado = "SP"
    } | ConvertTo-Json
    $script:plantio = Invoke-RestMethod -Uri "$BaseUrl/api/plantios" -Method Post -Headers $Headers `
        -Body $body -ContentType "application/json"
    Write-Host "   Plantio id=$($plantio.id)"
}

Test-Step "GET /api/plantios" {
    $plantios = Invoke-RestMethod -Uri "$BaseUrl/api/plantios" -Method Get -Headers $Headers
    Write-Host "   Total=$($plantios.Count)"
}

Test-Step "POST /api/historico" {
    $body = @{
        culturaId = 3
        culturaNome = "Milho"
        textoRecomendacao = "Plante entre setembro e novembro."
        score = 87.5
        latitude = -23.5505
        longitude = -46.6333
        detalhes = "Area irrigada"
    } | ConvertTo-Json
    $script:historico = Invoke-RestMethod -Uri "$BaseUrl/api/historico" -Method Post -Headers $Headers `
        -Body $body -ContentType "application/json"
    Write-Host "   Historico id=$($historico.id)"
}

Test-Step "GET /api/historico" {
    $historicos = Invoke-RestMethod -Uri "$BaseUrl/api/historico" -Method Get -Headers $Headers
    Write-Host "   Total=$($historicos.Count)"
}

Write-Host ""
Write-Host "=== Todos os testes passaram! ===" -ForegroundColor Green
