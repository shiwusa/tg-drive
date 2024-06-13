using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TgDrive.DataAccess.EntityFrameworkCore;

namespace TgDrive.Infrastructure.Services;

public static class ContextServiceCollectionExtensions
{
    public static IServiceCollection AddTgDriveContextPool(
        this IServiceCollection services,
        string mySqlConnectionStr)
    {
        return services.AddDbContextPool<TgDriveContext>(options =>
            options.UseMySql(
                mySqlConnectionStr,
                // ServerVersion.AutoDetect(mySqlConnectionStr),
                new MySqlServerVersion(new Version(8, 0, 35)),
                options => options
                    .MigrationsAssembly("TgDrive.DataAccess.MySqlMigrations")
                    .EnableRetryOnFailure()
        ));
    }
}
