namespace isc_tmr_backend.Features.Tasks.Application.Queries;

using isc_tmr_backend.Infrastructure.Presentation;
using FluentResults;
using MediatR;

public record GetPagedTasksQuery(
    RequestPagination Pagination,
    RequestOrderBy OrderBy,
    Guid? ProjectId = null,
    Guid? AssigneeId = null,
    string? Search = null
) : IRequest<Result<PagedResult<GetTaskResponse>>>;
