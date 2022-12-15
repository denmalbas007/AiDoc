using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AiDoc.Platform.Data.Factories;
using AiDoc.Platform.Data.Factories.Abstractions;
using AiDoc.Platform.Extensions;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AiDoc.Platform.Data.FluentMigrator;

public static class MigrationExtension
{
    public static IServiceCollection AddMigrator(this IServiceCollection services, Assembly assembly)
    {
        var servicesProvider = services.BuildServiceProvider();
        var connectionString =
            servicesProvider.GetService<IPostgresConnectionFactory>()?.GetConnection().ConnectionString;

        return services.AddFluentMigratorCore()
            .ConfigureRunner(
                x
                    => x.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(assembly).For.Migrations())
            .AddLogging(y => y.AddFluentMigratorConsole());
    }
    
    public static IServiceCollection AddMigrator<T>(this IServiceCollection services, Assembly assembly) where T : ICustomConnectionFactory
    {
        var servicesProvider = services.BuildServiceProvider();
        var connectionString =
            servicesProvider.GetService<IPostgresConnectionFactory<T>>()?.GetConnection().ConnectionString;

        return services.AddFluentMigratorCore()
            .ConfigureRunner(
                x
                    => x.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(assembly).For.Migrations())
            .AddLogging(y => y.AddFluentMigratorConsole());
    }

    public static Task RunOrMigrateAsync(this WebApplication app, string[] args)
    {
        if (!args.Contains("migrate") && !PlatformEnvironment.IsMigrate)
            return app.RunAsync();
        var iapp = (IApplicationBuilder)app;
        using var scope = iapp.ApplicationServices.CreateScope();
        var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
        runner!.ListMigrations();
        runner.MigrateUp();
        return Task.CompletedTask;
    }
}
