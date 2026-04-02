namespace isc_tmr_backend.Features.Activities;



public static class ActivitiesModule
{
    public static IServiceCollection AddActivitiesServices(this IServiceCollection services)
    {

        return services;
    }
    
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = ActivitiesController.Create(app); // ← delega la creación del grupo

        ActivitiesController.MapActivityEndpoints(group);

        return app;
    }
}