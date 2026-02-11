using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace isc.time.report.be.api.Security
{
    public class ModuleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public ModuleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            // Permitir endpoints públicos
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            Guid transacction = Guid.NewGuid();

            var user = context.User;

            var path = context.Request.Path.Value?.ToLower() ?? "";
            var method = context.Request.Method.ToUpper();
            var request = context.Request; 
            var headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
            var body = request.Body;
            var query = request.QueryString.Value;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Response.StatusCode = 401;
                return;
            }

            var modulesClaim = user.FindFirst("modules")?.Value;

            if (string.IsNullOrWhiteSpace(modulesClaim))
            {
                context.Response.StatusCode = 403;
                return;
            }

            List<string>? allowedModules;

            try
            {
                allowedModules = JsonSerializer.Deserialize<List<string>>(modulesClaim);
            }
            catch
            {
                context.Response.StatusCode = 403;
                return;
            }

            var requiredModule = ResolveModuleFromRequest(context);

            // Si no corresponde a un módulo protegido → permitir
            if (string.IsNullOrEmpty(requiredModule))
            {
                await _next(context);
                return;
            }

            var hasAccess = allowedModules!.Any(m =>
                m.Equals(requiredModule, StringComparison.OrdinalIgnoreCase)
            );

            if (!hasAccess)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync($"Acceso denegado al módulo {requiredModule}");
                return;
            }

            await _next(context);
        }

        private string ResolveModuleFromRequest(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // quitar /api
            if (path.StartsWith("/api"))
                path = path.Substring(4);

            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length == 0)
                return "";

            var controller = segments[0];

            // pluralización automática simple
            var module = controller switch
            {
                "employee" => "/employees",
                "client" => "/clients",
                "leader" => "/leaders",
                "user" => "/users",
                "holiday" => "/holidays",
                "project" => "/projects",
                "activity" => "/activities",
                "dashboard" => "/dashboard",
                "reports" => $"/reports/{segments.ElementAtOrDefault(1)}",
                "requirements" => $"/requirements/{segments.ElementAtOrDefault(1)}",
                "humanresources" => $"/humanresources/{segments.ElementAtOrDefault(1)}",
                _ => "/" + controller
            };

            return module;
        }
    }
}