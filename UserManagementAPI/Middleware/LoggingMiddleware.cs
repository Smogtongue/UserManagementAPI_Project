using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the request
        _logger.LogInformation($"Handling request: {context.Request.Method} {context.Request.Path}");

        // Call the next middleware in the pipeline
        await _next(context);

        // Log the response
        _logger.LogInformation($"Response: {context.Response.StatusCode}");
    }
}

// Extension method used to add the middleware to the HTTP request pipeline
public static class LoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}