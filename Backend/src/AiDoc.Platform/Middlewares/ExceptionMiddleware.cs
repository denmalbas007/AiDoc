using System;
using System.Threading.Tasks;
using AiDoc.Platform.Dtos;
using AiDoc.Platform.Exceptions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AiDoc.Platform.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    [UsedImplicitly]
    public static bool ReturnStackTrace { get; set; }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ExceptionWithCode ex)
        {
            var stackTrace = ReturnStackTrace ? ex.StackTrace : null;
            _logger.LogError(ex, "Exception occured during handling request");
            context.Response.StatusCode = ex.HttpCode;
            var error = new HttpError(ex.Message, stackTrace);
            await context.Response.WriteAsJsonAsync(error);
        }
        catch (Exception ex)
        {
            var stackTrace = ReturnStackTrace ? ex.StackTrace : null;
            _logger.LogError(ex, "Exception occured during handling request");
            context.Response.StatusCode = 500;
            var error = new HttpError(ex.Message, stackTrace);
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
