namespace isc_tmr_backend.Features.Projects.Application.Commands;

using FluentResults;
using MediatR;

public record UpdateProjectCommand(Guid Id, UpdateProjectRequest Request) : IRequest<Result<UpdateProjectResponse>>;

public record UpdateProjectRequest(string Name, string Description);

public record UpdateProjectResponse(Guid Id, string Name, string Description, DateTime UpdatedAt);
