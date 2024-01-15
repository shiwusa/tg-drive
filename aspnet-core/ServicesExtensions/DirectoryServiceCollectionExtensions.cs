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
        var collectionContainsMapper =
            services.Where(x => x.ServiceType == typeof(IMapper)).Any();
        if (!collectionContainsMapper)
        {
            services.AddMapper();
        }

        return services
            .AddScoped<IDirectoryRepository, DirectoryRepository>()
            .AddScoped<IDirectoryService, DirectoryService>();
    }
}
