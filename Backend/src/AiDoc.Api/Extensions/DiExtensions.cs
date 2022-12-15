using AiDoc.Api.Services.Authorization;
using AiDoc.Api.Services.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace AiDoc.Api.Extensions;

public static class DiExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IAuthorizationService, AuthorizationService>()
            .AddScoped<IDocumentsService, DocumentsService>();
}