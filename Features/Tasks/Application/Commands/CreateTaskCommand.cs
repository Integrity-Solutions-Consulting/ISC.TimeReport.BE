namespace isc_tmr_backend.Features.Tasks.Application.Commands;

using FluentResults;
using MediatR;

public record CreateTaskCommand(CreateTaskRequest Request) : IRequest<Result<CreateTaskResponse>>;

public record CreateTaskRequest(string Title, string Description, Guid ProjectId, Guid CreatedBy, Guid? AssigneeId);

public record CreateTaskResponse(Guid Id, string Title, string Description, bool IsCompleted, Guid ProjectId, Guid? AssigneeId, Guid CreatedBy, DateTime CreatedAt);
