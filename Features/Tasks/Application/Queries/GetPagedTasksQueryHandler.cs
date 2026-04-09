namespace isc_tmr_backend.Features.Tasks.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Tasks.Domain;
using isc_tmr_backend.Infrastructure.Presentation;
using MediatR;

public class GetPagedTasksQueryHandler : IRequestHandler<GetPagedTasksQuery, Result<PagedResult<GetTaskResponse>>>
{
    private readonly ITaskRepository _repository;

    public GetPagedTasksQueryHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResult<GetTaskResponse>>> Handle(GetPagedTasksQuery query, CancellationToken cancellationToken)
    {
        Result<PagedResult<TaskItem>> result = await _repository.GetPagedAsync(
            query.Page,
            query.Take,
            query.ProjectId,
            query.AssigneeId,
            query.SortBy,
            query.Ascending,
            query.Search,
            cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        IEnumerable<GetTaskResponse> response = result.Value.Data.Select(t => new GetTaskResponse(
            t.Id,
            t.Title,
            t.Description,
            t.IsCompleted,
            t.ProjectId,
            t.AssigneeId,
            t.CreatedBy,
            t.CreatedAt
        ));

        return Result.Ok(new PagedResult<GetTaskResponse>(response, result.Value.TotalCount));
    }
}
