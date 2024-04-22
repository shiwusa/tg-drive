using System.Reflection;
using ConsoleHost.Exceptions;
using DriveServices;
using DriveServices.EventServices;
using DriveServices.Implementations;
using DriveServices.Messages;
using EfRepositories;
using EfRepositories.Repositories;
using LiteDB;
using MappingConfig;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories;
using ServicesExtensions;
using Telegram.Bot;
using TgChatsStorage;
using TgFrontend;
using TgFrontend.Abstractions;
using TgFrontend.Menus;
using TgGateway.Abstractions;
using TgGateway.Implementations;

var mySqlConnectionStr =
    Environment.GetEnvironmentVariable("TGDRIVE_MYSQL_CONNECTION_STRING");
if (mySqlConnectionStr == null)
{
    throw new EnvironmentException("connection string for MySql",
        "TGDRIVE_MYSQL_CONNECTION_STRING");
}

var liteDbConnectionStr =
    Environment.GetEnvironmentVariable("TGDRIVE_LITEDB_CONNECTION_STRING");
if (liteDbConnectionStr == null)
{
    throw new EnvironmentException("connection string for LiteDb",
        "TGDRIVE_LITEDB_CONNECTION_STRING");
}

var tgBotToken =
    Environment.GetEnvironmentVariable("TGDRIVE_BOT_TOKEN");
if (tgBotToken == null)
{
    throw new EnvironmentException("telegram bot token",
        "TGDRIVE_BOT_TOKEN");
}

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TgDriveEventService>();
            x.SetKebabCaseEndpointNameFormatter();
            x.SetInMemorySagaRepositoryProvider();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq-bus",
                    "/",
                    h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                cfg.ConfigureEndpoints(context);
            });
        });

        services
            .AddTgDriveContextPool(mySqlConnectionStr)
            .AddMapper()
            .AddFileService()
            .AddDirectoryService()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ITgFileService, TgFileService>();

        services
            .AddScoped<ILiteDatabase>(_ => new LiteDatabase(liteDbConnectionStr))
            .AddScoped<IMessageStorage, LiteDbMessageStorage>();
        var bot = new TelegramBotClient(tgBotToken);
        services
            .AddSingleton<ITelegramBotClient>(bot)
            .AddSingleton<ITgDriveBotClient, TgDriveBotClient>();

        services.AddSingleton<IRedirectHandler, RedirectHandler>();
        services
            .AddSingleton<RootMenu>()
            .AddSingleton<SettingsMenu>()
            .AddSingleton<DirectoryMenu>()
            .AddSingleton<FileMenu>();

        services.AddSingleton<BotFrontend>();
    })
    .Build();

using (var db = host.Services.GetRequiredService<TgDriveContext>())
{
    db.Database.Migrate();
}

var front = host.Services.GetRequiredService<BotFrontend>();
front.Start();
await host.RunAsync();
