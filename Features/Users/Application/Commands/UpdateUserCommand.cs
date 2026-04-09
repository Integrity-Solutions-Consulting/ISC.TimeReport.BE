namespace isc_tmr_backend.Features.Users.Application.Commands;

using FluentResults;
using MediatR;

public record UpdateUserCommand(Guid Id, UpdateUserRequest Request) : IRequest<Result<UpdateUserResponse>>;

public record UpdateUserRequest(string Email, string DisplayName);

public record UpdateUserResponse(Guid Id, string Email, string DisplayName, DateTime UpdatedAt);
