namespace isc_tmr_backend.Features.Tasks.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Tasks.Domain;
using MediatR;

public class GetTasksByAssigneeQueryHandler : IRequestHandler<GetTasksByAssigneeQuery, Result<IEnumerable<GetTaskResponse>>>
{
    private readonly ITaskRepository _repository;

    public GetTasksByAssigneeQueryHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<GetTaskResponse>>> Handle(GetTasksByAssigneeQuery query, CancellationToken cancellationToken)
    {
        Result<IEnumerable<TaskItem>> result = await _repository.GetByAssigneeIdAsync(query.AssigneeId, cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        IEnumerable<GetTaskResponse> response = result.Value.Select(t => new GetTaskResponse(
            t.Id,
            t.Title,
            t.Description,
            t.IsCompleted,
            t.ProjectId,
            t.AssigneeId,
            t.CreatedBy,
            t.CreatedAt
        ));

        return Result.Ok(response);
    }
}
