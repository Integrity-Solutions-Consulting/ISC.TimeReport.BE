namespace isc_tmr_backend.Features.Users.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Users.Domain;
using MediatR;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<GetUserResponse>>>
{
    private readonly IUserRepository _repository;

    public GetAllUsersQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<GetUserResponse>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        Result<IEnumerable<User>> result = await _repository.GetAllAsync(cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        IEnumerable<GetUserResponse> response = result.Value.Select(u => new GetUserResponse(
            u.Id,
            u.Email,
            u.DisplayName,
            u.CreatedAt
        ));

        return Result.Ok(response);
    }
}
