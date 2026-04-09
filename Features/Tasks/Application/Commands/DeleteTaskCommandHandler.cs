namespace isc_tmr_backend.Features.Tasks.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Tasks.Domain;
using MediatR;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result<bool>>
{
    private readonly ITaskRepository _repository;

    public DeleteTaskCommandHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        Result deleteResult = await _repository.DeleteAsync(command.Id, cancellationToken);

        if (deleteResult.IsFailed) return Result.Fail(deleteResult.Errors);

        return Result.Ok(true);
    }
}
