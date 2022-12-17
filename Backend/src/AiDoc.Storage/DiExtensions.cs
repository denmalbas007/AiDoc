using Microsoft.Extensions.DependencyInjection;

namespace AiDoc.Storage;

public static class DiExtensions
{
    public static IServiceCollection AddFileStorage(this IServiceCollection services)
        => services
            .AddScoped<IFileStorage, PgFileStorage>()
            .AddScoped<IFileStoragePgRepository, FileStoragePgRepository>();
}