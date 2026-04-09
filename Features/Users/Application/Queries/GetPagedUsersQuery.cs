namespace isc_tmr_backend.Features.Users.Application.Queries;

using FluentResults;
using isc_tmr_backend.Infrastructure.Presentation;
using MediatR;

public record GetPagedUsersQuery(
    int Page = 1,
    int Take = 10,
    string? SortBy = null,
    bool Ascending = true,
    string? Search = null
) : IRequest<Result<PagedResult<GetUserResponse>>>;
