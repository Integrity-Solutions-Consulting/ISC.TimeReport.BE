namespace isc_tmr_backend.Features.Tasks.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Tasks.Domain;
using MediatR;

public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, Result<CompleteTaskResponse>>
{
    private readonly ITaskRepository _repository;

    public CompleteTaskCommandHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CompleteTaskResponse>> Handle(CompleteTaskCommand command, CancellationToken cancellationToken)
    {
        Result<TaskItem> taskResult = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (taskResult.IsFailed) return Result.Fail(taskResult.Errors);

        TaskItem task = taskResult.Value;
        task.MarkAsCompleted();

        Result<TaskItem> updateResult = await _repository.UpdateAsync(task, cancellationToken);

        if (updateResult.IsFailed) return Result.Fail(updateResult.Errors);

        return Result.Ok(new CompleteTaskResponse(
            task.Id,
            task.IsCompleted,
            task.UpdatedAt
        ));
    }
}
