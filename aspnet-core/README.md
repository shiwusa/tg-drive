# Tg drive

This project provides a virtual file system with unlimited storage for each user based on Telegram API. The user has the ability to add, delete, move and share files using a simple and easy-to-understand interface right from his favorite messenger.

## Quick Start

**Firstly, you need to install .NET Core on your PC** ([instructions here](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu))

Example (Ubuntu 20.04 LTS):
```
$ wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

$ sudo dpkg -i packages-microsoft-prod.deb

$ rm packages-microsoft-prod.deb
```

**To build the project you need to go forward into solution directory and run:**

```
$ dotnet restore

$ dotnet build
```

## Running the bot

To run the bot hosting you need to setup these enviroment variables:

- TGDRIVE_MYSQL_CONNECTION_STRING
- TGDRIVE_LITEDB_CONNECTION_STRING
- TGDRIVE_BOT_TOKEN

How to set these variables you can read in folowing manuals:

https://www.litedb.org/docs/getting-started/

https://core.telegram.org/

https://dev.mysql.com/doc/mysql-getting-started/en/

Next you go forward into BotHost directory and then run the hosting with following command:

```
$ dotnet run
```
## License

MIT

## Contributors

- Ivan Baturkin - [gurug-prog](https://github.com/gurug-prog)
- Matthew Kirik - [MatthewKirik](https://github.com/MatthewKirik)
