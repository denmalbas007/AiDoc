using AiDoc.Api.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace AiDoc.Api.Infrastructure.ScopeDisposer;

public static class DiExtensions
{
    public static IServiceCollection AddScopeDisposer(this IServiceCollection services)
        => services
            .AddScoped<IScopeDisposer, ScopeDisposer>()
            .AddScoped<IUsersService, UsersService>();
}