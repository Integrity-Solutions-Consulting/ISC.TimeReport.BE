namespace isc_tmr_backend.Features.Projects.Endpoint;

using FluentValidation;
using FluentValidation.Results;
using isc_tmr_backend.Features.Projects.Application.Commands;
using isc_tmr_backend.Features.Projects.Application.Queries;
using isc_tmr_backend.Infrastructure.Extensions;
using isc_tmr_backend.Infrastructure.Presentation;
using MediatR;
using System.Net;

public static class ProjectEndpoint
{
    public static RouteGroupBuilder MapProjectEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/projects", GetAllProjects)
            .WithName("GetProjects")
            .WithTags("Projects")
            .Produces<ResponseWithMetadata<IEnumerable<GetProjectResponse>>>(200);

        group.MapGet("/projects/{id:guid}", GetProjectById)
            .WithName("GetProjectById")
            .WithTags("Projects")
            .Produces<ResponseWithMetadata<GetProjectResponse>>(200)
            .Produces(404);

        group.MapGet("/projects/owner/{ownerId:guid}", GetProjectsByOwner)
            .WithName("GetProjectsByOwner")
            .WithTags("Projects")
            .Produces<ResponseWithMetadata<IEnumerable<GetProjectResponse>>>(200);

        group.MapPost("/projects", CreateProject)
            .WithName("CreateProject")
            .WithTags("Projects")
            .Produces<ResponseWithMetadata<CreateProjectResponse>>(201)
            .ProducesValidationProblem();

        group.MapPut("/projects/{id:guid}", UpdateProject)
            .WithName("UpdateProject")
            .WithTags("Projects")
            .Produces<ResponseWithMetadata<UpdateProjectResponse>>(200)
            .ProducesValidationProblem()
            .Produces(404);

        group.MapDelete("/projects/{id:guid}", DeleteProject)
            .WithName("DeleteProject")
            .WithTags("Projects")
            .Produces<ResponseWithMetadata<bool>>(200)
            .Produces(404);

        return group;
    }

    private static async Task<IResult> GetAllProjects(
        int page = 1,
        int take = 10,
        Guid? ownerId = null,
        string? sortBy = null,
        bool ascending = true,
        string? search = null,
        IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPagedProjectsQuery(page, take, ownerId, sortBy, ascending, search);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToPagedResponse("Projects retrieved successfully", page, take);
    }

    private static async Task<IResult> GetProjectById(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProjectByIdQuery(id), cancellationToken);
        return result.ToSuccessResponse("Project retrieved successfully", HttpStatusCode.OK);
    }

    private static async Task<IResult> GetProjectsByOwner(
        Guid ownerId,
        int page = 1,
        int take = 10,
        string? sortBy = null,
        bool ascending = true,
        string? search = null,
        IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPagedProjectsQuery(page, take, ownerId, sortBy, ascending, search);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToPagedResponse("Projects retrieved successfully", page, take);
    }

    private static async Task<IResult> CreateProject(CreateProjectRequest request, IValidator<CreateProjectCommand> validator, IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new CreateProjectCommand(request);

        ValidationResult validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

        var result = await mediator.Send(command, cancellationToken);
        return result.ToSuccessResponse("Project created successfully", HttpStatusCode.Created);
    }

    private static async Task<IResult> UpdateProject(Guid id, UpdateProjectRequest request, IValidator<UpdateProjectCommand> validator, IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new UpdateProjectCommand(id, request);

        ValidationResult validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

        var result = await mediator.Send(command, cancellationToken);
        return result.ToSuccessResponse("Project updated successfully", HttpStatusCode.OK);
    }

    private static async Task<IResult> DeleteProject(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProjectCommand(id), cancellationToken);
        return result.ToSuccessResponse("Project deleted successfully", HttpStatusCode.OK);
    }
}
