namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Features.Notifications;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var versionedApi = app
            .NewVersionedApi()
            .MapGroup(app.Configuration["Server:Prefix"] ?? "/api");

        NotificationModule.MapEndpoints(versionedApi);

        return app;
    }
}