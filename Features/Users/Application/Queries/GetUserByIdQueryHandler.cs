namespace isc_tmr_backend.Features.Users.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Users.Domain;
using MediatR;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<GetUserResponse>>
{
    private readonly IUserRepository _repository;

    public GetUserByIdQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetUserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        Result<User> result = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        User user = result.Value;

        return Result.Ok(new GetUserResponse(
            user.Id,
            user.Email,
            user.DisplayName,
            user.CreatedAt
        ));
    }
}
