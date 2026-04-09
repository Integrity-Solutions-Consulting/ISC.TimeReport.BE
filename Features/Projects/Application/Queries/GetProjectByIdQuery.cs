namespace isc_tmr_backend.Features.Projects.Application.Queries;

using FluentResults;
using MediatR;

public record GetProjectByIdQuery(Guid Id) : IRequest<Result<GetProjectResponse>>;
