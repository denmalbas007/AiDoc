using AiDoc.Api.DataAccess.Repositories.Extensions;
using AiDoc.Api.Extensions;
using AiDoc.Api.Infrastructure.DictCache;
using AiDoc.Api.Infrastructure.ScopeDisposer;
using AiDoc.Ml.Client;
using AiDoc.Platform.Data.FluentMigrator;
using AiDoc.Platform.Extensions;
using AiDoc.Platform.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication
    .CreateBuilder(args)
    .UsePlatform();
var services = builder.Services;
var configuration = builder.Configuration;

#region DI

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwagger("ai-doc-api", useJwtAuth: true);
services.AddSerilogLogger();
services.AddHttpContextAccessor();
services.AddServices();
services.AddDataAccess();
services.AddScopeDisposer();
services.AddHttpClient();
services.AddScoped<MlApiClient>();
services.AddSingleton<IDictCache<string, int>, DictCache<string, int>>();
services.AddPlatformAuthentication(configuration);
services.AddAuthorization();

#endregion

var app = builder.Build();

#region App

ExceptionMiddleware.ReturnStackTrace = false;
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(
    x =>
    {
        x.AllowAnyHeader();
        x.AllowAnyMethod();
        x.AllowAnyOrigin();
    });
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

#endregion

await app.RunOrMigrateAsync(args);