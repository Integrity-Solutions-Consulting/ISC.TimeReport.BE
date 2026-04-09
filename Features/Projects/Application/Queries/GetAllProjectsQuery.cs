namespace isc_tmr_backend.Features.Projects.Application.Queries;

using FluentResults;
using MediatR;

public record GetAllProjectsQuery : IRequest<Result<IEnumerable<GetProjectResponse>>>;

public record GetProjectResponse(Guid Id, string Name, string Description, Guid OwnerId, DateTime CreatedAt);
