using AutoMapper;
using DriveServices.Implementations;
using DriveServices;
using EfRepositories.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Repositories;

namespace ServicesExtensions;

public static class DirectoryServiceCollectionExtensions
{
    public static IServiceCollection AddDirectoryService(this IServiceCollection services)
    {
        var collectionContainsMapper = services.Any(x => x.ServiceType == typeof(IMapper));
        if (!collectionContainsMapper)
        {
            services.AddMapper();
        }

        return services
            .AddScoped<IDirectoryRepository, DirectoryRepository>()
            .AddScoped<IDirectoryService, DirectoryService>();
    }
}
