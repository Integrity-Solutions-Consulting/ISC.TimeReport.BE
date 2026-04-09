namespace isc_tmr_backend.Features.Tasks.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Tasks.Domain;
using MediatR;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<CreateTaskResponse>>
{
    private readonly ITaskRepository _repository;

    public CreateTaskCommandHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CreateTaskResponse>> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        TaskItem task = TaskItem.Create(
            command.Request.Title,
            command.Request.Description,
            command.Request.ProjectId,
            command.Request.CreatedBy,
            command.Request.AssigneeId
        );

        Result<TaskItem> result = await _repository.AddAsync(task, cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        return Result.Ok(new CreateTaskResponse(
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
