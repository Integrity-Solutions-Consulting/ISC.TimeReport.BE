namespace isc_tmr_backend.Features.Tasks.Application.Queries;

using FluentResults;
using MediatR;

public record GetAllTasksQuery : IRequest<Result<IEnumerable<GetTaskResponse>>>;

public record GetTaskResponse(Guid Id, string Title, string Description, bool IsCompleted, Guid ProjectId, Guid? AssigneeId, Guid CreatedBy, DateTime CreatedAt);
