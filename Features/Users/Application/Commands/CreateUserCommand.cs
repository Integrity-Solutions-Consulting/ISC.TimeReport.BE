namespace isc_tmr_backend.Features.Users.Application.Commands;

using FluentResults;
using MediatR;

public record CreateUserCommand(CreateUserRequest Request) : IRequest<Result<CreateUserResponse>>;

public record CreateUserRequest(string Email, string DisplayName);

public record CreateUserResponse(Guid Id, string Email, string DisplayName, DateTime CreatedAt);
