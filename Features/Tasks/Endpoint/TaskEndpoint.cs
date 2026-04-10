namespace isc_tmr_backend.Features.Tasks.Endpoint;

using FluentValidation;
using FluentValidation.Results;
using isc_tmr_backend.Features.Tasks.Application.Commands;
using isc_tmr_backend.Features.Tasks.Application.Queries;
using isc_tmr_backend.Infrastructure.Extensions;
using isc_tmr_backend.Infrastructure.Presentation;
using MediatR;
using System.Net;

public static class TaskEndpoint
{
    public static RouteGroupBuilder MapTaskEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/tasks", GetAllTasks)
            .WithName("GetTasks")
            .WithTags("Tasks")
            .Produces<ResponseWithMetadata<IEnumerable<GetTaskResponse>>>(200);

        group.MapGet("/tasks/assignee/{assigneeId:guid}", GetTasksByAssignee)
            .WithName("GetTasksByAssignee")
            .WithTags("Tasks")
            .Produces<ResponseWithMetadata<IEnumerable<GetTaskResponse>>>(200);

        group.MapGet("/projects/{projectId:guid}/tasks", GetTasksByProject)
            .WithName("GetTasksByProject")
            .WithTags("Tasks")
            .Produces<ResponseWithMetadata<IEnumerable<GetTaskResponse>>>(200);

        group.MapGet("/projects/{projectId:guid}/tasks/{id:guid}", GetTaskById)
            .WithName("GetTaskById")
            .WithTags("Tasks")
            .Produces<ResponseWithMetadata<GetTaskResponse>>(200)
            .Produces(404);

        group.MapPost("/projects/{projectId:guid}/tasks", CreateTask)
            .WithName("CreateTask")
            .WithTags("Tasks")
            .Produces<ResponseWithMetadata<CreateTaskResponse>>(201)
            .ProducesValidationProblem();

        group.MapPut("/projects/{projectId:guid}/tasks/{id:guid}", UpdateTask)
            .WithName("UpdateTask")
            .WithTags("Tasks")
            .Produces<ResponseWithMetadata<UpdateTaskResponse>>(200)
            .ProducesValidationProblem()
            .Produces(404);

        group.MapPatch("/projects/{projectId:guid}/tasks/{id:guid}/complete", CompleteTask)
            .WithName("CompleteTask")
            .WithTags("Tasks")
            .Produces<ResponseWithMetadata<CompleteTaskResponse>>(200)
            .Produces(404);

        group.MapDelete("/projects/{projectId:guid}/tasks/{id:guid}", DeleteTask)
            .WithName("DeleteTask")
            .WithTags("Tasks")
            .Produces<ResponseWithMetadata<bool>>(200)
            .Produces(404);

        return group;
    }

    private static async Task<IResult> GetAllTasks([AsParameters] RequestPagination pagination, [AsParameters] RequestOrderBy orderBy,
        Guid? projectId = null,
        Guid? assigneeId = null,
        string? search = null,
        IMediator mediator = null!,
        CancellationToken cancellationToken = default
    )
    {
        GetPagedTasksQuery? query = new(pagination, orderBy, projectId, assigneeId,  search);

        var result = await mediator.Send(query, cancellationToken);

        return result.ToPagedResponse("Tasks retrieved successfully", pagination.Page, pagination.Take);
    }
    
    private static async Task<IResult> GetTasksByProject([AsParameters] RequestPagination pagination, [AsParameters] RequestOrderBy orderBy,
        Guid projectId,
        int page = 1,
        int take = 10,
        Guid? assigneeId = null,
        string? sortBy = null,
        bool ascending = true,
        string? search = null,
        IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        GetPagedTasksQuery? query = new(pagination, orderBy, projectId, assigneeId,  search);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToPagedResponse("Tasks retrieved successfully", page, take);
    }

    private static async Task<IResult> GetTasksByAssignee([AsParameters] RequestPagination pagination, [AsParameters] RequestOrderBy orderBy,
        Guid assigneeId,
        int page = 1,
        int take = 10,
        Guid? projectId = null,
        string? search = null,
        IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        GetPagedTasksQuery? query = new(pagination, orderBy, projectId, assigneeId,  search);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToPagedResponse("Tasks retrieved successfully", page, take);
    }

    private static async Task<IResult> GetTaskById(Guid projectId, Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetTaskByIdQuery(id), cancellationToken);
        return result.ToSuccessResponse("Task retrieved successfully", HttpStatusCode.OK);
    }

    private static async Task<IResult> CreateTask(Guid projectId, CreateTaskRequest request, IValidator<CreateTaskCommand> validator, IMediator mediator, CancellationToken cancellationToken)
    {
        var commandRequest = new CreateTaskRequest(
            request.Title,
            request.Description,
            projectId,
            request.CreatedBy,
            request.AssigneeId
        );
        var command = new CreateTaskCommand(commandRequest);

        ValidationResult validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

        var result = await mediator.Send(command, cancellationToken);
        return result.ToSuccessResponse("Task created successfully", HttpStatusCode.Created);
    }

    private static async Task<IResult> UpdateTask(Guid projectId, Guid id, UpdateTaskRequest request, IValidator<UpdateTaskCommand> validator, IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new UpdateTaskCommand(id, request);

        ValidationResult validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

        var result = await mediator.Send(command, cancellationToken);
        return result.ToSuccessResponse("Task updated successfully", HttpStatusCode.OK);
    }

    private static async Task<IResult> CompleteTask(Guid projectId, Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CompleteTaskCommand(id), cancellationToken);
        return result.ToSuccessResponse("Task completed successfully", HttpStatusCode.OK);
    }

    private static async Task<IResult> DeleteTask(Guid projectId, Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteTaskCommand(id), cancellationToken);
        return result.ToSuccessResponse("Task deleted successfully", HttpStatusCode.OK);
    }
}
