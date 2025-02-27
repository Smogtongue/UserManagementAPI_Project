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
        _logger.LogInformation("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);

        // Copy the original response body stream
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await _next(context);

            // Log the response
            _logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);

            // Copy the contents of the new memory stream (which contains the response) to the original stream
            await responseBody.CopyToAsync(originalBodyStream);
        }
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