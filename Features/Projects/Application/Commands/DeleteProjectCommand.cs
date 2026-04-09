namespace isc_tmr_backend.Features.Projects.Application.Commands;

using FluentResults;
using MediatR;

public record DeleteProjectCommand(Guid Id) : IRequest<Result<bool>>;
