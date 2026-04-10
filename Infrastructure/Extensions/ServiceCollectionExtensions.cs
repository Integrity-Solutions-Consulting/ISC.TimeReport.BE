namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Features.Auth;
using isc_tmr_backend.Features.Tasks;
using isc_tmr_backend.Features.Projects;
using isc_tmr_backend.Features.Users;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUsersServices();
        services.AddTasksServices();
        services.AddProjectsServices();

        return services;
    }
}
