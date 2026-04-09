namespace isc_tmr_backend.Features.Tasks.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Tasks.Domain;
using MediatR;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<GetTaskResponse>>
{
    private readonly ITaskRepository _repository;

    public GetTaskByIdQueryHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetTaskResponse>> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        Result<TaskItem> result = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        TaskItem task = result.Value;

        return Result.Ok(new GetTaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.IsCompleted,
            task.ProjectId,
            task.AssigneeId,
            task.CreatedBy,
            task.CreatedAt
        ));
    }
}
