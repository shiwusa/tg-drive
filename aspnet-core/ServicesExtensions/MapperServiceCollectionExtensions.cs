using Microsoft.Extensions.DependencyInjection;
using TgDrive.Infrastructure.AutoMapper;

namespace TgDrive.Infrastructure.Services;

public static class MapperServiceCollectionExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddSingleton(AutoMapperConfigurator.GetMapper());
    }
}
