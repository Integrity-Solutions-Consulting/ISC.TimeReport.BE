using isc.time.report.be.application.Interfaces.Service.Auth;
using isc.time.report.be.application.Interfaces.Service.Clients;
using isc.time.report.be.application.Interfaces.Service.DailyActivities;
using isc.time.report.be.application.Interfaces.Service.Employees;
using isc.time.report.be.application.Interfaces.Service.Leaders;
using isc.time.report.be.application.Interfaces.Service.Menus;
using isc.time.report.be.application.Interfaces.Service.Permissions;
using isc.time.report.be.application.Interfaces.Service.PermissionTypes;
using isc.time.report.be.application.Interfaces.Service.Persons;
using isc.time.report.be.application.Interfaces.Service.Projects;
using isc.time.report.be.application.Interfaces.Service.TimeReports;
using isc.time.report.be.application.Interfaces.Service.Users;
using isc.time.report.be.application.Services;
using isc.time.report.be.application.Services.Auth;
using isc.time.report.be.application.Services.Clients;
using isc.time.report.be.application.Services.DailyActivities;
using isc.time.report.be.application.Services.Employees;
using isc.time.report.be.application.Services.Leaders;
using isc.time.report.be.application.Services.Menus;
using isc.time.report.be.application.Services.Permissions;
using isc.time.report.be.application.Services.PermissionTypes;
using isc.time.report.be.application.Services.Persons;
using isc.time.report.be.application.Services.Projects;
using isc.time.report.be.application.Services.TimeReports;
using isc.time.report.be.application.Services.Users;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Emails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.IOC
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<PasswordUtils>();
            services.AddSingleton<JWTUtils>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ILeaderService, LeaderService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPermissionTypeService, PermissionTypeService>();
            services.AddScoped<IDailyActivityService,DailyActivityService>();
            services.AddScoped<ITimeReportService, TimeReportService>();


            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<EmailService>();


            return services;
        }
    }
}
