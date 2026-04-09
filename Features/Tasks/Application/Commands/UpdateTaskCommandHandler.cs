namespace isc_tmr_backend.Features.Tasks.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Tasks.Domain;
using MediatR;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<UpdateTaskResponse>>
{
    private readonly ITaskRepository _repository;

    public UpdateTaskCommandHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UpdateTaskResponse>> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        Result<TaskItem> taskResult = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (taskResult.IsFailed) return Result.Fail(taskResult.Errors);

        TaskItem task = taskResult.Value;
        task.Update(command.Request.Title, command.Request.Description, command.Request.AssigneeId);

        Result<TaskItem> updateResult = await _repository.UpdateAsync(task, cancellationToken);

        if (updateResult.IsFailed) return Result.Fail(updateResult.Errors);

        return Result.Ok(new UpdateTaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.IsCompleted,
            task.AssigneeId,
            task.UpdatedAt
        ));
    }
}
