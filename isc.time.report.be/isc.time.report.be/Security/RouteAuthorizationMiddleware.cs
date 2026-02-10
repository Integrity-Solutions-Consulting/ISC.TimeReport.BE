using System.Text.Json;

namespace isc.time.report.be.api.Security
{
    public class RouteAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RouteAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;

            // Si no está autenticado → 401
            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // Obtener módulos del token
            var modulesClaim = user.FindFirst("modules")?.Value;

            if (string.IsNullOrWhiteSpace(modulesClaim))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            List<string>? allowedModules;

            try
            {
                allowedModules = JsonSerializer.Deserialize<List<string>>(modulesClaim);
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            if (allowedModules == null || allowedModules.Count == 0)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            // Ruta solicitada
            var requestPath = context.Request.Path.Value?.ToLower() ?? "";

            // Validar si tiene acceso a la ruta
            var hasAccess = allowedModules.Any(module =>
                requestPath.Contains(module.ToLower())
            );

            if (!hasAccess)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("No tienes permiso para acceder a este endpoint.");
                return;
            }

            await _next(context);
        }
    }
}