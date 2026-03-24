namespace isc_tmr_backend.Features.Notifications;

using FluentValidation;
using isc_tmr_backend.Features.Notifications.Application;
using isc_tmr_backend.Features.Notifications.Application.Commands;
using isc_tmr_backend.Features.Notifications.Domain;
using isc_tmr_backend.Features.Notifications.Endpoint;
using isc_tmr_backend.Features.Notifications.Infrastructure;
using isc_tmr_backend.Features.Notifications.Infrastructure.Repositories;

public static class NotificationModule
{
    public static IServiceCollection AddNotificationServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationRepository, NotificationsRepository>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<ISqsPublisher, SqsPublisher>();
        services.AddScoped<IValidator<SendNotificationCommand>, SendNotificationValidator>();
        return services;
    }
    
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = NotificationRouteGroup.Create(app); // ← delega la creación del grupo

        SendNotificationEndpoint.MapNotificationEndpoints(group);

        return app;
    }
}