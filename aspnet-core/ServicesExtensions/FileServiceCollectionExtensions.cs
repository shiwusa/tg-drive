using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TgDrive.Domain.Services.Implementations;
using TgDrive.Domain.Services;
using TgDrive.DataAccess.EntityFrameworkCore;
using TgDrive.DataAccess.Shared;

namespace TgDrive.Infrastructure.Services;
public static class FileServiceCollectionExtensions
{
    public static IServiceCollection AddFileService(this IServiceCollection services)
    {
        var collectionContainsMapper = services.Any(x => x.ServiceType == typeof(IMapper));
        if (!collectionContainsMapper)
        {
            services.AddMapper();
        }

        return services
            .AddScoped<IFileRepository, FileRepository>()
            .AddScoped<IFileService, FileService>();
    }
}
