namespace isc_tmr_backend.Features.Tasks.Application.Queries;

using FluentResults;
using MediatR;

public record GetTasksByProjectQuery(Guid ProjectId) : IRequest<Result<IEnumerable<GetTaskResponse>>>;
