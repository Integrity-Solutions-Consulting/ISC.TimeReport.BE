namespace isc_tmr_backend.Features.Tasks.Application.Commands;

using FluentResults;
using MediatR;

public record DeleteTaskCommand(Guid Id) : IRequest<Result<bool>>;
