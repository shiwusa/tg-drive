#!/bin/bash

# Configuration Guide:
# https://aka.ms/dev-certs-trust

export TGDRIVE_CERT_PATH=/usr/local/share/ca-certificates/aspnet

if [ "$1" = "--clean" ]; then
    sudo rm $TGDRIVE_CERT_PATH/tgdrive.pfx
fi

if [ ! -e "$TGDRIVE_CERT_PATH/tgdrive.pfx" ]; then
    dotnet dev-certs https --clean
    sudo -E dotnet dev-certs https -ep $TGDRIVE_CERT_PATH/tgdrive.pfx -p awesomepass
    dotnet dev-certs https --check

    certutil -d sql:$HOME/.pki/nssdb -A -t "P,," -n localhost -i $TGDRIVE_CERT_PATH/tgdrive.pfx
    certutil -d sql:$HOME/.pki/nssdb -A -t "C,," -n localhost -i $TGDRIVE_CERT_PATH/tgdrive.pfx
else
    echo "TgDrive certificate already exists!"
fi

if [ "$2" = "--build" ]; then
    docker compose up -d --build
else
    docker compose up -d
fi
