namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Features.Notifications;
using isc_tmr_backend.Features.Activities;

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

        NotificationModule.MapEndpoints(versionedApi);
        ActivitiesModule.MapEndpoints(versionedApi);

        return app;
    }


    // configura el middleware que captura excepciones no controladas
    // y las convierte automáticamente al formato RFC 7807
    // equivalente al @ExceptionHandler global de Spring
    public static WebApplication UseProblemDetailsConfig(this WebApplication app)
    {
        // captura cualquier excepción no controlada en toda la app
        // sin este middleware las excepciones retornarían HTML en lugar de JSON
        app.UseExceptionHandler();

        // retorna RFC 7807 para cualquier respuesta de error sin body
        // por ejemplo un 404 de una ruta que no existe
        app.UseStatusCodePages();

        return app;
    }
}