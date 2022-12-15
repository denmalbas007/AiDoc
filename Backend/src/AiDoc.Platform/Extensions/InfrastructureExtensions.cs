using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace AiDoc.Platform.Extensions;

[PublicAPI]
public static class InfrastructureExtensions
{
    public static IServiceCollection AddSerilogLogger(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .CreateLogger();

        return services.AddSingleton(Log.Logger);
    }

    public static WebApplicationBuilder WithLocalConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile("appsettings.Local.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
            .AddEnvironmentVariables();

        return builder;
    }
}
