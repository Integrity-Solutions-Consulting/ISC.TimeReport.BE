namespace isc_tmr_backend.Features.Users.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Users.Domain;
using MediatR;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
{
    private readonly IUserRepository _repository;

    public DeleteUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        Result deleteResult = await _repository.DeleteAsync(command.Id, cancellationToken);

        if (deleteResult.IsFailed) return Result.Fail(deleteResult.Errors);

        return Result.Ok(true);
    }
}
