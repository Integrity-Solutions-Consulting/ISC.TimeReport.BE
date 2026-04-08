namespace isc_tmr_backend.Features.Activities;

using isc_tmr_backend.Features.Activities.Application.Queries;
using isc_tmr_backend.Infrastructure.Presentation;
using FluentResults;
using MediatR;
using System.Net;

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
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.OK);

        group.MapGet("/activities/{id}", GetActivity)
            .WithName("GetActivity")
            .WithTags("Activities")
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.OK);

        group.MapPost("/activities", PostActivities)
            .WithName("PostActivities")
            .WithTags("Activities")
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.Created);

        group.MapDelete("/activities", DeleteActivities)
            .WithName("DeleteActivities")
            .WithTags("Activities")
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.OK);

        group.MapPut("/activities", PutActivities)
            .WithName("PutActivities")
            .WithTags("Activities")
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.OK);

        return group;
    }

    private static async Task<IResult> GetActivity(string id, IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllActivitiesQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }


    private static async Task<IResult> GetActivities(IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllActivitiesQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }

    private static async Task<IResult> PostActivities(IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllActivitiesQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }

    private static async Task<IResult> DeleteActivities(IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllActivitiesQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }

    private static async Task<IResult> PutActivities(IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllActivitiesQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }
}