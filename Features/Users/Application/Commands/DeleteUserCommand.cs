namespace isc_tmr_backend.Features.Users.Application.Commands;

using FluentResults;
using MediatR;

public record DeleteUserCommand(Guid Id) : IRequest<Result<bool>>;
