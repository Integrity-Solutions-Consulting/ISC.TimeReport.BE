namespace isc_tmr_backend.Features.Projects.Application.Queries;

using FluentResults;
using isc_tmr_backend.Infrastructure.Presentation;
using MediatR;

public record GetPagedProjectsQuery(
    int Page = 1,
    int Take = 10,
    Guid? OwnerId = null,
    string? SortBy = null,
    bool Ascending = true,
    string? Search = null
) : IRequest<Result<PagedResult<GetProjectResponse>>>;
