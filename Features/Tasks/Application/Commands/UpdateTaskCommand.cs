namespace isc_tmr_backend.Features.Tasks.Application.Commands;

using FluentResults;
using MediatR;

public record UpdateTaskCommand(Guid Id, UpdateTaskRequest Request) : IRequest<Result<UpdateTaskResponse>>;

public record UpdateTaskRequest(string Title, string Description, Guid? AssigneeId);

public record UpdateTaskResponse(Guid Id, string Title, string Description, bool IsCompleted, Guid? AssigneeId, DateTime UpdatedAt);
