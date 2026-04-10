namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Features.Auth;
using isc_tmr_backend.Features.Tasks;
using isc_tmr_backend.Features.Projects;
using isc_tmr_backend.Features.Users;

public static class WebApplicationExtensions
{
    public static WebApplication UseMapEndpoints(this WebApplication app)
    {
        RouteGroupBuilder versionedApi = app
            .NewVersionedApi()
            .MapGroup(app.Configuration["Server:Prefix"] ?? "/api")
            .ProducesValidationProblem(300)
            .ProducesValidationProblem(400)
            .ProducesValidationProblem(500);

        UsersModule.MapEndpoints(versionedApi);
        TasksModule.MapEndpoints(versionedApi);
        AuthModule.MapAuthEndpoints(versionedApi);
        ProjectsModule.MapEndpoints(versionedApi);

        return app;
    }

}
