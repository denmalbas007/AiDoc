using System.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

namespace AiDoc.Platform.Extensions;

[PublicAPI]
public static class PlatformExtensions
{
    public static WebApplicationBuilder UsePlatform(this WebApplicationBuilder builder)
    {
        builder.WithLocalConfiguration();
        builder.Host.UseSerilog();
        return builder.ConfigurePorts();
    }

    private static WebApplicationBuilder ConfigurePorts(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(
            options =>
            {
                Listen(options, PlatformEnvironment.HttpPort, HttpProtocols.Http1);
                Listen(options, PlatformEnvironment.GrpcPort, HttpProtocols.Http2);
                Listen(options, PlatformEnvironment.DebugPort, HttpProtocols.Http1);
            });

        void Listen(KestrelServerOptions kestrelServerOptions, int? port, HttpProtocols protocols)
        {
            if (!port.HasValue)
                return;

            var address = PlatformEnvironment.IsRunningInContainer
                ? IPAddress.Any
                : IPAddress.Loopback;

            kestrelServerOptions.Listen(address, port.Value, listenOptions => { listenOptions.Protocols = protocols; });
        }

        return builder;
    }
}
