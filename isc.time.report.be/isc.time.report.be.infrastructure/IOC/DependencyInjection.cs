using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Repositories.Auth;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace isc.time.report.be.infrastructure.IOC
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();

            return services;
        }

        public static IServiceCollection AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            string conection = configuration["ConecctionStrings:ConexionBD"]!.ToString();

            var username = Environment.GetEnvironmentVariable("FACTURACION_BD_USER");
            var password = Environment.GetEnvironmentVariable("FACTURACION_BD_PASSWORD");

            var conecctionBuilder = new SqlConnectionStringBuilder(conection)
            {
                Password = password,
                UserID = username,
            };

            services.AddDbContext<DBContext>(options => options.UseSqlServer(conecctionBuilder.ConnectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            return services;
        }






    }
}
