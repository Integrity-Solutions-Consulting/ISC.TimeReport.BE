namespace isc_tmr_backend.Features.Tasks.Application.Queries;

using FluentResults;
using isc_tmr_backend.Infrastructure.Presentation;
using MediatR;

public record GetPagedTasksQuery(
    int Page = 1,
    int Take = 10,
    Guid? ProjectId = null,
    Guid? AssigneeId = null,
    string? SortBy = null,
    bool Ascending = true,
    string? Search = null
) : IRequest<Result<PagedResult<GetTaskResponse>>>;
