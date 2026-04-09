namespace isc_tmr_backend.Features.Todo;

using isc_tmr_backend.Features.Todo.Application.Queries;
using isc_tmr_backend.Infrastructure.Presentation;
using FluentResults;
using MediatR;
using System.Net;

public static class TodoController
{

    public static RouteGroupBuilder Create(IEndpointRouteBuilder app)
    {
        return app
            .MapGroup("")
            .HasApiVersion(1, 0);
    }

    public static RouteGroupBuilder MapTodoEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/todos", GetTodos)
            .WithName("GetTodos")
            .WithTags("Todos")
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.OK);

        group.MapGet("/todos/{id}", GetTodo)
            .WithName("GetTodo")
            .WithTags("Todos")
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.OK);

        group.MapPost("/todos", PostTodos)
            .WithName("PostTodos")
            .WithTags("Todos")
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.Created);

        group.MapDelete("/todos", DeleteTodos)
            .WithName("DeleteTodos")
            .WithTags("Todos")
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.OK);

        group.MapPut("/todos", PutTodos)
            .WithName("PutTodos")
            .WithTags("Todos")
            .Produces<ResponseWithMetadata<IEnumerable<string>>>((int)HttpStatusCode.OK);

        return group;
    }

    private static async Task<IResult> GetTodo(string id, IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllTodoQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }


    private static async Task<IResult> GetTodos(IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllTodoQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }

    private static async Task<IResult> PostTodos(IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllTodoQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }

    private static async Task<IResult> DeleteTodos(IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllTodoQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }

    private static async Task<IResult> PutTodos(IMediator mediator, CancellationToken cancellationToken)
    {

        Result<IEnumerable<string>> result = await mediator.Send(new GetAllTodoQuery(), cancellationToken);

        // aquí iría la lógica para obtener las actividades, por ahora devolvemos un ejemplo
        IEnumerable<string> activities = new List<string> { "Activity 1", "Activity 2", "Activity 3" };
        return Results.Ok(activities);
    }
}