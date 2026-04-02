namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Features.Notifications;
using isc_tmr_backend.Features.Activities;
public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        RouteGroupBuilder versionedApi = app
            .NewVersionedApi()
            .MapGroup(app.Configuration["Server:Prefix"] ?? "/api");

        NotificationModule.MapEndpoints(versionedApi);
        ActivitiesModule.MapEndpoints(versionedApi);

        return app;
    }
}