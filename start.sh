#!/bin/bash

CERTIFICATES_PATH=/usr/local/share/ca-certificates/aspnet

if [ ! -e "$CERTIFICATES_PATH/tgdrive.pfx" ]; then
    dotnet dev-certs https --clean
    dotnet dev-certs https -ep $CERTIFICATES_PATH/tgdrive.pfx -p awesomepass --format PEM
    dotnet dev-certs https --check

    certutil -d sql:$HOME/.pki/nssdb -A -t "P,," -n localhost -i $CERTIFICATES_PATH/tgdrive.pfx
    certutil -d sql:$HOME/.pki/nssdb -A -t "C,," -n localhost -i $CERTIFICATES_PATH/tgdrive.pfx
else
    echo "TgDrive certificate already exists!"
fi

docker compose up -d
