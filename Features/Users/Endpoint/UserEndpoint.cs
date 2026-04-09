namespace isc_tmr_backend.Features.Users.Endpoint;

using FluentValidation;
using FluentValidation.Results;
using isc_tmr_backend.Features.Users.Application.Commands;
using isc_tmr_backend.Features.Users.Application.Queries;
using isc_tmr_backend.Infrastructure.Extensions;
using isc_tmr_backend.Infrastructure.Presentation;
using MediatR;
using System.Net;

public static class UserEndpoint
{
    public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/users", GetAllUsers)
            .WithName("GetUsers")
            .WithTags("Users")
            .Produces<ResponseWithMetadata<IEnumerable<GetUserResponse>>>(200);

        group.MapGet("/users/{id:guid}", GetUserById)
            .WithName("GetUserById")
            .WithTags("Users")
            .Produces<ResponseWithMetadata<GetUserResponse>>(200)
            .Produces(404);

        group.MapPost("/users", CreateUser)
            .WithName("CreateUser")
            .WithTags("Users")
            .Produces<ResponseWithMetadata<CreateUserResponse>>(201)
            .ProducesValidationProblem();

        group.MapPut("/users/{id:guid}", UpdateUser)
            .WithName("UpdateUser")
            .WithTags("Users")
            .Produces<ResponseWithMetadata<UpdateUserResponse>>(200)
            .ProducesValidationProblem()
            .Produces(404);

        group.MapDelete("/users/{id:guid}", DeleteUser)
            .WithName("DeleteUser")
            .WithTags("Users")
            .Produces<ResponseWithMetadata<bool>>(200)
            .Produces(404);

        return group;
    }

    private static async Task<IResult> GetAllUsers(
        int page = 1,
        int take = 10,
        string? sortBy = null,
        bool ascending = true,
        string? search = null,
        IMediator mediator = null!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPagedUsersQuery(page, take, sortBy, ascending, search);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToPagedResponse("Users retrieved successfully", page, take);
    }

    private static async Task<IResult> GetUserById(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        return result.ToSuccessResponse("User retrieved successfully", HttpStatusCode.OK);
    }

    private static async Task<IResult> CreateUser(CreateUserRequest request, IValidator<CreateUserCommand> validator, IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(request);

        ValidationResult validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

        var result = await mediator.Send(command, cancellationToken);
        return result.ToSuccessResponse("User created successfully", HttpStatusCode.Created);
    }

    private static async Task<IResult> UpdateUser(Guid id, UpdateUserRequest request, IValidator<UpdateUserCommand> validator, IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(id, request);

        ValidationResult validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

        var result = await mediator.Send(command, cancellationToken);
        return result.ToSuccessResponse("User updated successfully", HttpStatusCode.OK);
    }

    private static async Task<IResult> DeleteUser(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return result.ToSuccessResponse("User deleted successfully", HttpStatusCode.OK);
    }
}
