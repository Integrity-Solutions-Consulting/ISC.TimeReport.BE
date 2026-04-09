namespace isc_tmr_backend.Features.Auth.Infrastructure;

public class StubAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<StubAuthorizationMiddleware> _logger;

    public StubAuthorizationMiddleware(RequestDelegate next, ILogger<StubAuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogDebug("StubAuthorizationMiddleware is processing request: {Method} {Path}", 
            context.Request.Method, context.Request.Path);

        await _next(context);
    }
}

public static class StubAuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseStubAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<StubAuthorizationMiddleware>();
    }
}
