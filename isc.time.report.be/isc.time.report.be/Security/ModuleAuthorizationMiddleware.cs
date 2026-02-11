using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace isc.time.report.be.api.Security
{
    public class ModuleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ModuleSecurityOptions _options;
        private readonly ILogger<ModuleAuthorizationMiddleware> _logger;

        public ModuleAuthorizationMiddleware(
            RequestDelegate next,
            IOptions<ModuleSecurityOptions> options,
            ILogger<ModuleAuthorizationMiddleware> logger)
        {
            _next = next;
            _options = options.Value;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            var user = context.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                await Deny(context, 401, "No autenticado");
                return;
            }

            var path = NormalizePath(context.Request.Path.Value);

            // 🔹 NULL SAFE IgnoreRoutes
            var ignoreRoutes = _options.IgnoreRoutes ?? new List<string>();

            if (ignoreRoutes.Any(r => path.StartsWith(r)))
            {
                await _next(context);
                return;
            }

            var roleId = user.FindFirst("roleId")?.Value;

            if (string.IsNullOrEmpty(roleId))
            {
                await Deny(context, 403, "Token inválido (sin rol)");
                return;
            }

            // 🔹 NULL SAFE RoleModules
            if (_options.RoleModules == null ||
                !_options.RoleModules.TryGetValue(roleId, out var roleModules))
            {
                await Deny(context, 403, "Rol sin permisos");
                return;
            }

            roleModules ??= new List<string>();

            if (roleModules.Contains("*"))
            {
                await _next(context);
                return;
            }

            var requiredModule = ResolveModuleFromPath(path);

            if (string.IsNullOrEmpty(requiredModule))
            {
                await _next(context);
                return;
            }

            if (!roleModules.Contains(requiredModule))
            {
                await Deny(context, 403, $"Sin acceso al módulo {requiredModule}");
                return;
            }

            if (IsOwnResourceEndpoint(context))
            {
                if (!IsAccessingOwnResource(context))
                {
                    await Deny(context, 403, "Solo puede acceder a su propio registro");
                    return;
                }
            }

            await _next(context);
        }

        private string NormalizePath(string? path)
        {
            if (string.IsNullOrEmpty(path)) return "";

            path = path.ToLower();

            if (path.StartsWith("/api"))
                path = path.Substring(4);

            return path;
        }

        private string ResolveModuleFromPath(string path)
        {
            var moduleRoutes = _options.ModuleRoutes ?? new Dictionary<string, List<string>>();

            foreach (var module in moduleRoutes)
            {
                var moduleName = module.Key.ToLower();
                var routes = module.Value ?? new List<string>();

                foreach (var apiPrefix in routes)
                {
                    if (path.StartsWith(apiPrefix.ToLower()))
                        return moduleName;
                }
            }

            return "";
        }

        private bool IsOwnResourceEndpoint(HttpContext context)
        {
            var path = NormalizePath(context.Request.Path.Value);
            var method = context.Request.Method.ToUpper();

            // SOLO aplica a consulta por ID
            return method == "GET" &&
                   path.StartsWith("/employee/getemployeebyid/");
        }

        private bool IsAccessingOwnResource(HttpContext context)
        {
            var employeeIdClaim = context.User.FindFirst("EmployeeID")?.Value;

            if (string.IsNullOrEmpty(employeeIdClaim))
                return false;

            var routeId = context.Request.RouteValues["id"]?.ToString();

            if (string.IsNullOrEmpty(routeId))
                return true; // endpoint "GetMyEmployee"

            return routeId == employeeIdClaim;
        }

        private async Task Deny(HttpContext context, int status, string message)
        {
            _logger.LogWarning("Acceso denegado → {Path} | {Message}",
                context.Request.Path, message);

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}