using AiDoc.Platform.Data.Factories;
using AiDoc.Platform.Data.Factories.Abstractions;
using AiDoc.Platform.Data.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AiDoc.Platform.Data.Extensions;

public static class DataAccessExtensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        services.AddScoped<IPostgresConnectionFactory, PostgresConnectionFactory>();
        services.TryAddScoped<IDbConnectionsProvider, DbConnectionsProvider>();
        services.TryAddScoped<IDbTransactionsProvider, DbTransactionsProvider>();
        return services;
    }

    public static IServiceCollection AddCustomPostgres<T>(this IServiceCollection services) where T : ICustomConnectionFactory
    {
        services.AddScoped<IPostgresConnectionFactory<T>, PostgresConnectionFactory<T>>();
        services.TryAddScoped<IDbConnectionsProvider, DbConnectionsProvider>();
        services.TryAddScoped<IDbTransactionsProvider, DbTransactionsProvider>();
        return services;
    }
}