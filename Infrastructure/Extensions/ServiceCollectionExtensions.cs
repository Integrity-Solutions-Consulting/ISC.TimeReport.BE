namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Features.Notifications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Slices
        services.AddNotificationServices();

        return services;
    }
}