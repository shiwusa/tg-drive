using MappingConfig;
using Microsoft.Extensions.DependencyInjection;

namespace ServicesExtensions;
public static class MapperServiceCollectionExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddSingleton(AutoMapperConfigurator.GetMapper());
    }
}
