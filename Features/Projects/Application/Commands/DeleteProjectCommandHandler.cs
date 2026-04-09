namespace isc_tmr_backend.Features.Projects.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Projects.Domain;
using MediatR;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result<bool>>
{
    private readonly IProjectRepository _repository;

    public DeleteProjectCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        Result deleteResult = await _repository.DeleteAsync(command.Id, cancellationToken);

        if (deleteResult.IsFailed) return Result.Fail(deleteResult.Errors);

        return Result.Ok(true);
    }
}
