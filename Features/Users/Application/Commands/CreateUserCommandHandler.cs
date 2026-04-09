namespace isc_tmr_backend.Features.Users.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Users.Domain;
using MediatR;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        User user = User.Create(command.Request.Email, command.Request.DisplayName);

        Result<User> result = await _repository.AddAsync(user, cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        return Result.Ok(new CreateUserResponse(
            user.Id,
            user.Email,
            user.DisplayName,
            user.CreatedAt
        ));
    }
}
