namespace isc_tmr_backend.Features.Projects.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Projects.Domain;
using MediatR;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<CreateProjectResponse>>
{
    private readonly IProjectRepository _repository;

    public CreateProjectCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CreateProjectResponse>> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        Project project = Project.Create(command.Request.Name, command.Request.Description, command.Request.OwnerId);

        Result<Project> result = await _repository.AddAsync(project, cancellationToken);

        if (result.IsFailed) return Result.Fail(result.Errors);

        return Result.Ok(new CreateProjectResponse(
            project.Id,
            project.Name,
            project.Description,
            project.OwnerId,
            project.CreatedAt
        ));
    }
}
