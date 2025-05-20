using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Service.Auth;
using isc.time.report.be.application.Services.Auth;
using isc.time.report.be.application.Utils.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace isc.time.report.be.application.IOC
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<PasswordUtils>();
            services.AddSingleton<JWTUtils>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
