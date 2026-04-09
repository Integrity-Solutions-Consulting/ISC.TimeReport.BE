namespace isc_tmr_backend.Features.Tasks.Application.Queries;

using FluentResults;
using MediatR;

public record GetTasksByAssigneeQuery(Guid AssigneeId) : IRequest<Result<IEnumerable<GetTaskResponse>>>;
