@REM @ECHO OFF

SET "certificates_path=%USERPROFILE%\.aspnet\https"

IF NOT EXIST "%certificates_path%\tgdrive.pfx" (
    dotnet dev-certs https --clean
    dotnet dev-certs https -ep %certificates_path%\tgdrive.pfx -p awesomepass
    dotnet dev-certs https --trust
) ELSE (
    ECHO "TgDrive certificate already exists!"
)

docker compose up -d
