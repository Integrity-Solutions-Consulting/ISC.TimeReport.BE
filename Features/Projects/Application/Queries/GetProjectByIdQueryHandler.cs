namespace isc_tmr_backend.Features.Projects.Application.Queries;

using FluentResults;
using isc_tmr_backend.Features.Projects.Domain;
using MediatR;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<GetProjectResponse>>
{
    private readonly IProjectRepository _repository;

    public GetProjectByIdQueryHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetProjectResponse>> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken)
    {
        Result<Project> result = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        Project project = result.Value;

        return Result.Ok(new GetProjectResponse(
            project.Id,
            project.Name,
            project.Description,
            project.OwnerId,
            project.CreatedAt
        ));
    }
}
