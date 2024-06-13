using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TgDrive.DataAccess.EntityFrameworkCore;
using TgDrive.DataAccess.Shared;
using TgDrive.Domain.Services;
using TgDrive.Domain.Services.Implementations;

namespace TgDrive.Infrastructure.Services;

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
