namespace isc_tmr_backend.Features.Notifications;

public static class NotificationRouteGroup
{
    public static RouteGroupBuilder Create(IEndpointRouteBuilder app)
    {
        return app
            .MapGroup("")
            .HasApiVersion(1, 0);
    }
}