using Microsoft.Extensions.DependencyInjection;
using TgDrive.Config.AutoMapper;

namespace TgDrive.Config.Services;
public static class MapperServiceCollectionExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddSingleton(AutoMapperConfigurator.GetMapper());
    }
}
