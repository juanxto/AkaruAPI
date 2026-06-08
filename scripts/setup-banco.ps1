# Sobe Oracle (Docker) e executa o banco GS completo + complemento .NET
# Uso: .\scripts\setup-banco.ps1

$ErrorActionPreference = "Stop"
$Root = Split-Path $PSScriptRoot -Parent
Set-Location $Root

$ConnectionString = "User Id=akaru;Password=akaru123;Data Source=localhost:1521/XEPDB1"
$SqlScripts = @(
    "01_ddl.sql",
    "02_dml.sql",
    "04_procedures_functions.sql",
    "05_triggers.sql",
    "06_package.sql",
    "03_blocos_anonimos.sql",
    "07_relatorios.sql",
    "09_dotnet_api_complement.sql"
)

function Write-Step([string]$Msg) {
    Write-Host ""
    Write-Host ">> $Msg" -ForegroundColor Cyan
}

function Test-DockerRunning {
    try {
        docker info *> $null
        return $LASTEXITCODE -eq 0
    } catch {
        return $false
    }
}

function Invoke-SqlInContainer([string]$ScriptName) {
    $containerPath = "/scripts/gs-db/$ScriptName"
    Write-Host "   Executando $ScriptName..." -ForegroundColor DarkGray
    docker exec akaru-oracle sqlplus -s akaru/akaru123@XEPDB1 "@$containerPath" 2>&1 | ForEach-Object {
        if ($_ -match "ERROR|ORA-|SP2-") { Write-Host $_ -ForegroundColor Red }
    }
    if ($LASTEXITCODE -ne 0) {
        throw "Falha ao executar $ScriptName"
    }
}

Write-Host "=== Akaru - Setup do banco Oracle ===" -ForegroundColor Yellow

Write-Step "Verificando Docker"
if (-not (Test-DockerRunning)) {
    Write-Host "Docker nao esta rodando." -ForegroundColor Red
    Write-Host "Abra o Docker Desktop e rode este script novamente." -ForegroundColor Yellow
    exit 1
}
Write-Host "   Docker OK" -ForegroundColor Green

Write-Step "Subindo Oracle XE (primeira vez pode levar 2-3 min)"
docker compose up -d oracle

Write-Step "Aguardando Oracle ficar pronto"
$maxWait = 300
$elapsed = 0
while ($elapsed -lt $maxWait) {
    $health = docker inspect --format='{{.State.Health.Status}}' akaru-oracle 2>$null
    if ($health -eq "healthy") {
        Write-Host "   Oracle healthy!" -ForegroundColor Green
        break
    }
    Write-Host ("   Aguardando... " + $elapsed + "s") -ForegroundColor DarkGray
    Start-Sleep -Seconds 15
    $elapsed += 15
}
if ($elapsed -ge $maxWait) {
    Write-Host "Oracle nao ficou pronto a tempo. Verifique: docker logs akaru-oracle" -ForegroundColor Red
    exit 1
}

Write-Step "Executando scripts do banco GS"
foreach ($script in $SqlScripts) {
    if (-not (Test-Path "scripts\gs-db\$script")) {
        throw "Script nao encontrado: $script"
    }
    Invoke-SqlInContainer $script
}
Write-Host "   Scripts executados!" -ForegroundColor Green

Write-Step "Verificando tabelas"
$checkSql = @"
SET PAGESIZE 0 FEEDBACK OFF
SELECT table_name FROM user_tables ORDER BY table_name;
EXIT
"@
$tables = $checkSql | docker exec -i akaru-oracle sqlplus -s akaru/akaru123@XEPDB1
Write-Host $tables

Write-Step "Atualizando appsettings.Development.json"
$devSettingsPath = "src\Akaru.API\appsettings.Development.json"
$devSettings = @{
    ConnectionStrings = @{ Oracle = $ConnectionString }
    Firebase = @{
        ProjectId = "akaru-app"
        CredentialsPath = "firebase-credentials.json"
        UseMockAuth = $true
    }
    Logging = @{
        LogLevel = @{
            Default = "Information"
            "Microsoft.AspNetCore" = "Warning"
            "Microsoft.EntityFrameworkCore" = "Warning"
        }
    }
} | ConvertTo-Json -Depth 4
$devSettings | Set-Content -Path $devSettingsPath -Encoding UTF8
Write-Host "   Connection string configurada" -ForegroundColor Green

Write-Host ""
Write-Host "=== Banco pronto! ===" -ForegroundColor Green
Write-Host "Connection string: $ConnectionString"
Write-Host ""
Write-Host "Proximo passo:"
Write-Host "  cd src\Akaru.API"
Write-Host "  dotnet run"
Write-Host "  .\scripts\testar-api.ps1"
