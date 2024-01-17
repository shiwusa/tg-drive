@ECHO OFF

SET "TGDRIVE_CERT_PATH=%USERPROFILE%\.aspnet\https"

if "%1"=="--clean" (
    DEL "%TGDRIVE_CERT_PATH%\tgdrive.pfx"
)

IF NOT EXIST "%TGDRIVE_CERT_PATH%\tgdrive.pfx" (
    dotnet dev-certs https --clean
    dotnet dev-certs https -ep %TGDRIVE_CERT_PATH%\tgdrive.pfx -p awesomepass
    dotnet dev-certs https --trust
) ELSE (
    ECHO "TgDrive certificate already exists!"
)

IF "%2"=="--build" (
    docker compose up -d --build
) ELSE (
    docker compose up -d
)
