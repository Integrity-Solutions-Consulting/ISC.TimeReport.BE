namespace isc_tmr_backend.Features.Projects.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Projects.Domain;
using MediatR;

public class GetProjectsByOwnerQueryHandler : IRequestHandler<GetProjectsByOwnerQuery, Result<IEnumerable<GetProjectResponse>>>
{
    private readonly IProjectRepository _repository;

    public GetProjectsByOwnerQueryHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<GetProjectResponse>>> Handle(GetProjectsByOwnerQuery query, CancellationToken cancellationToken)
    {
        Result<IEnumerable<Project>> result = await _repository.GetByOwnerIdAsync(query.OwnerId, cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        IEnumerable<GetProjectResponse> response = result.Value.Select(p => new GetProjectResponse(
            p.Id,
            p.Name,
            p.Description,
            p.OwnerId,
            p.CreatedAt
        ));

        return Result.Ok(response);
    }
}
