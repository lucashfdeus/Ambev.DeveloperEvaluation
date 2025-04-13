@echo off
:: Script para iniciar o PostgreSQL com a porta 5432 fixa no host
:: ------------------------------------------------

:: Configurações
set SERVICE_NAME=ambev.developerevaluation.database
set COMPOSE_FILE=.\docker-compose.yml
set POSTGRES_HOST_PORT=5432
set POSTGRES_CONTAINER_PORT=5432

:: Verifica se o Docker está rodando
docker info >nul 2>&1 || (
    echo [ERRO] Docker não está rodando ou não está instalado.
    pause
    exit /b 1
)

:: Força o mapeamento da porta no docker-compose.yml temporariamente
(
    echo version: '3.8'
    echo services:
    echo   %SERVICE_NAME%:
    echo     image: postgres:13
    echo     ports:
    echo       - "%POSTGRES_HOST_PORT%:%POSTGRES_CONTAINER_PORT%"
    echo     environment:
    echo       POSTGRES_DB: developer_evaluation
    echo       POSTGRES_USER: developer
    echo       POSTGRES_PASSWORD: ev@luAt10n
) > %COMPOSE_FILE%

:: Inicia o container
echo [INFO] Iniciando PostgreSQL na porta %POSTGRES_HOST_PORT%...
docker-compose -p ambev_developer_evaluation up -d %SERVICE_NAME%

if %ERRORLEVEL% neq 0 (
    echo [ERRO] Falha ao iniciar o container.
    pause
    exit /b 1
)

:: Verifica o status
echo [INFO] Container está rodando em:
docker ps --filter "name=%SERVICE_NAME%" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

:: Mensagem final
echo.
echo [SUCESSO] PostgreSQL pronto!
echo Conecte-se usando:
echo - Host: localhost
echo - Porta: %POSTGRES_HOST_PORT%
echo - Usuário: developer
echo - Senha: ev@luAt10n
echo.

pause