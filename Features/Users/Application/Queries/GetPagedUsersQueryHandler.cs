namespace isc_tmr_backend.Features.Users.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Users.Domain;
using isc_tmr_backend.Infrastructure.Presentation;
using MediatR;

public class GetPagedUsersQueryHandler : IRequestHandler<GetPagedUsersQuery, Result<PagedResult<GetUserResponse>>>
{
    private readonly IUserRepository _repository;

    public GetPagedUsersQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResult<GetUserResponse>>> Handle(GetPagedUsersQuery query, CancellationToken cancellationToken)
    {
        Result<PagedResult<User>> result = await _repository.GetPagedAsync(
            query.Page,
            query.Take,
            query.SortBy,
            query.Ascending,
            query.Search,
            cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        IEnumerable<GetUserResponse> response = result.Value.Data.Select(u => new GetUserResponse(
            u.Id,
            u.Email,
            u.DisplayName,
            u.CreatedAt
        ));

        return Result.Ok(new PagedResult<GetUserResponse>(response, result.Value.TotalCount));
    }
}
