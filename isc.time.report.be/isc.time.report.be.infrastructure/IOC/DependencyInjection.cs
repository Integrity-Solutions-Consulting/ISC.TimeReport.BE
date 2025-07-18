using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Catalogs;
using isc.time.report.be.application.Interfaces.Repository.Clients;
using isc.time.report.be.application.Interfaces.Repository.DailyActivities;
using isc.time.report.be.application.Interfaces.Repository.Dashboards;
using isc.time.report.be.application.Interfaces.Repository.Employees;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.application.Interfaces.Repository.Permissions;
using isc.time.report.be.application.Interfaces.Repository.PermissionTypes;
using isc.time.report.be.application.Interfaces.Repository.Persons;
using isc.time.report.be.application.Interfaces.Repository.Projects;
using isc.time.report.be.application.Interfaces.Repository.TimeReports;
using isc.time.report.be.application.Interfaces.Repository.Users;
using isc.time.report.be.application.Interfaces.Service.DailyActivities;
using isc.time.report.be.application.Interfaces.Service.Dashboards;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Repositories.Auth;
using isc.time.report.be.infrastructure.Repositories.Catalogs;
using isc.time.report.be.infrastructure.Repositories.Clients;
using isc.time.report.be.infrastructure.Repositories.DailyActivities;
using isc.time.report.be.infrastructure.Repositories.Dashboards;
using isc.time.report.be.infrastructure.Repositories.Employees;
using isc.time.report.be.infrastructure.Repositories.Leaders;
using isc.time.report.be.infrastructure.Repositories.Menus;
using isc.time.report.be.infrastructure.Repositories.Permissions;
using isc.time.report.be.infrastructure.Repositories.PermissionTypes;
using isc.time.report.be.infrastructure.Repositories.Persons;
using isc.time.report.be.infrastructure.Repositories.Projects;
using isc.time.report.be.infrastructure.Repositories.TimeReports;
using isc.time.report.be.infrastructure.Repositories.Users;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.IOC
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ILeaderRepository, LeaderRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
            services.AddScoped<IDailyActivityRepository, DailyActivityRepository>();
            services.AddScoped<ITimeReportRepository, TimeReportRepository>();
            services.AddScoped<ICatalogRepository, CatalogRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            return services;
        }

        public static IServiceCollection AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            string conection = configuration["ConecctionStrings:ConexionBD"]!.ToString();

            var username = Environment.GetEnvironmentVariable("ISC_TIME_REPORT_BD_USER");
            var password = Environment.GetEnvironmentVariable("ISC_TIME_REPORT_BD_PASSWORD");

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
