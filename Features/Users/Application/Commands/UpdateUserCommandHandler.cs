namespace isc_tmr_backend.Features.Users.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Users.Domain;
using MediatR;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UpdateUserResponse>>
{
    private readonly IUserRepository _repository;

    public UpdateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        Result<User> userResult = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (userResult.IsFailed) return Result.Fail(userResult.Errors);

        User user = userResult.Value;
        user.Update(command.Request.Email, command.Request.DisplayName);

        Result<User> updateResult = await _repository.UpdateAsync(user, cancellationToken);

        if (updateResult.IsFailed) return Result.Fail(updateResult.Errors);

        return Result.Ok(new UpdateUserResponse(
            user.Id,
            user.Email,
            user.DisplayName,
            user.UpdatedAt
        ));
    }
}
