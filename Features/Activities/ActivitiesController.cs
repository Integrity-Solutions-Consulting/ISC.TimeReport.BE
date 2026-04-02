namespace isc_tmr_backend.Features.Activities;

public static class ActivitiesController
{

    public static RouteGroupBuilder Create(IEndpointRouteBuilder app)
    {
        return app
            .MapGroup("")
            .HasApiVersion(1, 0);
    }
    
    public static RouteGroupBuilder MapActivityEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/activities", GetActivities)
             .WithName("GetActivities")
             .WithTags("Activities")
             .Produces<IEnumerable<string>>(200);

        return group;
    }

    private static IResult GetActivities()
    {
        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }
}