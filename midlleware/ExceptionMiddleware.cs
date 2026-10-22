us
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using webShop2.midlleware;

namespace webShop2.middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Greska pri obradi zahteva {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        if (context.Response.HasStarted)
        {
            throw exception;
        }

        int statusCode;

        if (exception is AppException appException)
        {
            statusCode = appException.StatusCode;
        }
        else
        {
            statusCode =
                StatusCodes.Status500InternalServerError;
        }

        string error = statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            422 => "Unprocessable Entity",
            500 => "Internal Server Error",
            _ => "Error"
        };

        context.Response.Clear();
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(new
        {
            status = statusCode,
            error = error,
            message = error
        });
    }
}
