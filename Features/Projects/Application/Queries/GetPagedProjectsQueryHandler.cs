namespace isc_tmr_backend.Features.Projects.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Projects.Domain;
using isc_tmr_backend.Infrastructure.Presentation;
using MediatR;

public class GetPagedProjectsQueryHandler : IRequestHandler<GetPagedProjectsQuery, Result<PagedResult<GetProjectResponse>>>
{
    private readonly IProjectRepository _repository;

    public GetPagedProjectsQueryHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResult<GetProjectResponse>>> Handle(GetPagedProjectsQuery query, CancellationToken cancellationToken)
    {
        Result<PagedResult<Project>> result = await _repository.GetPagedAsync(
            query.Page,
            query.Take,
            query.OwnerId,
            query.SortBy,
            query.Ascending,
            query.Search,
            cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        IEnumerable<GetProjectResponse> response = result.Value.Data.Select(p => new GetProjectResponse(
            p.Id,
            p.Name,
            p.Description,
            p.OwnerId,
            p.CreatedAt
        ));

        return Result.Ok(new PagedResult<GetProjectResponse>(response, result.Value.TotalCount));
    }
}
