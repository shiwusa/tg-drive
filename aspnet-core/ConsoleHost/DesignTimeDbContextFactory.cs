using TgDrive.DataAccess.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TgDrive.BotClient.Host;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TgDriveContext>
{
    public TgDriveContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();
        var builder = new DbContextOptionsBuilder<TgDriveContext>();

        var connectionString =
            configuration.GetConnectionString("DesignTimeConnectionString");

        Console.WriteLine(connectionString);
        builder.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            options => options.MigrationsAssembly("TgDrive.DataAccess.MySqlMigrations")
        );

        return new TgDriveContext(builder.Options);
    }
}
