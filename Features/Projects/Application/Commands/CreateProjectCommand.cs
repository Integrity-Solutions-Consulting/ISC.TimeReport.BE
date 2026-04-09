namespace isc_tmr_backend.Features.Projects.Application.Commands;

using FluentResults;
using MediatR;

public record CreateProjectCommand(CreateProjectRequest Request) : IRequest<Result<CreateProjectResponse>>;

public record CreateProjectRequest(string Name, string Description, Guid OwnerId);

public record CreateProjectResponse(Guid Id, string Name, string Description, Guid OwnerId, DateTime CreatedAt);
