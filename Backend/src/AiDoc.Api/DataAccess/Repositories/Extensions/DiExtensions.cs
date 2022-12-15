using AiDoc.Api.DataAccess.Migrations;
using AiDoc.Api.DataAccess.Repositories.User;
using AiDoc.Platform.Data.Extensions;
using AiDoc.Platform.Data.FluentMigrator;
using Microsoft.Extensions.DependencyInjection;

namespace AiDoc.Api.DataAccess.Repositories.Extensions;

public static class DiExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
        => services
            .AddPostgres()
            .AddScoped<IUserRepository, UserRepository>()
            .AddMigrator(typeof(InitMigration).Assembly);
}