using LiteDB;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using TgDrive.BotClient.Frontend;
using TgDrive.BotClient.Frontend.Abstractions;
using TgDrive.BotClient.Frontend.Menus;
using TgDrive.BotClient.Host;
using TgDrive.Infrastructure.LiteDB;
using TgDrive.Infrastructure.Services;
using TgDrive.DataAccess.Shared;
using TgDrive.DataAccess.EntityFrameworkCore;
using TgDrive.Domain.Services;
using TgDrive.Domain.Services.Implementations;
using TgDrive.Domain.Telegram.Abstractions;
using TgDrive.Domain.Telegram.Implementations;


var mySqlConnectionStr = Environment.GetEnvironmentVariable("TGDRIVE_MYSQL_CONNECTION_STRING")
    ?? throw new EnvironmentException(
        "connection string for MySql", "TGDRIVE_MYSQL_CONNECTION_STRING");

var liteDbConnectionStr = Environment.GetEnvironmentVariable("TGDRIVE_LITEDB_CONNECTION_STRING")
    ?? throw new EnvironmentException(
        "connection string for LiteDb", "TGDRIVE_LITEDB_CONNECTION_STRING");

var tgBotToken = Environment.GetEnvironmentVariable("TGDRIVE_BOT_TOKEN")
    ?? throw new EnvironmentException(
        "telegram bot token", "TGDRIVE_BOT_TOKEN");

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

        services
            .AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(tgBotToken))
            .AddSingleton<ITgDriveBotClient, TgDriveBotClient>();

        services.AddSingleton<IRedirectHandler, RedirectHandler>();
        services
            .AddSingleton<RootMenu>()
            .AddSingleton<SettingsMenu>()
            .AddSingleton<DirectoryMenu>()
            .AddSingleton<FileMenu>();

        services
            .AddSingleton<IUpdateHandler, UpdateHandler>()
            .AddSingleton<BotFrontend>();
    })
    .Build();

using (var dbContext = host.Services.GetRequiredService<TgDriveContext>())
{
    dbContext.Database.Migrate();
}

var front = host.Services.GetRequiredService<BotFrontend>();
front.Start();
await host.RunAsync();
