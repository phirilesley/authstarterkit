namespace AuthSystem.API.Middleware;

public sealed class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        logger.LogInformation("Incoming {Method} {Path}", context.Request.Method, context.Request.Path);
        await next(context);
    }
}
