namespace isc_tmr_backend.Features.Projects.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Projects.Domain;
using MediatR;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<UpdateProjectResponse>>
{
    private readonly IProjectRepository _repository;

    public UpdateProjectCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UpdateProjectResponse>> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        Result<Project> projectResult = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (projectResult.IsFailed) return Result.Fail(projectResult.Errors);

        Project project = projectResult.Value;
        project.Update(command.Request.Name, command.Request.Description);

        Result<Project> updateResult = await _repository.UpdateAsync(project, cancellationToken);

        if (updateResult.IsFailed) return Result.Fail(updateResult.Errors);

        return Result.Ok(new UpdateProjectResponse(
            project.Id,
            project.Name,
            project.Description,
            project.UpdatedAt
        ));
    }
}
