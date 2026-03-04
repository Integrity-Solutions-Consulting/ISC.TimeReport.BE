using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace isc.time.report.be.api.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRouteAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredModule;

        public AuthorizeRouteAttribute(string requiredModule)
        {
            _requiredModule = requiredModule;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // 1️⃣ Usuario no autenticado → 401
            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // 2️⃣ Obtener módulos del token
            var modulesClaim = user.FindFirst("modules")?.Value;

            if (string.IsNullOrWhiteSpace(modulesClaim))
            {
                context.Result = new ForbidResult(); // 403
                return;
            }

            List<string>? allowedModules;

            try
            {
                allowedModules = JsonSerializer.Deserialize<List<string>>(modulesClaim);
            }
            catch
            {
                context.Result = new ForbidResult();
                return;
            }

            // 3️⃣ Validar acceso al módulo requerido
            if (allowedModules == null || !allowedModules.Contains(_requiredModule))
            {
                context.Result = new ForbidResult(); // 403
            }
        }
    }
}