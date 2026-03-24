namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Features.Notifications.Application.Commands;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRConfig(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(SendNotificationCommandHandler).Assembly));

        return services;
    }
}