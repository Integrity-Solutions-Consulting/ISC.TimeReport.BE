namespace isc_tmr_backend.Features.Tasks.Application.Commands;

using FluentResults;
using MediatR;

public record CompleteTaskCommand(Guid Id) : IRequest<Result<CompleteTaskResponse>>;

public record CompleteTaskResponse(Guid Id, bool IsCompleted, DateTime UpdatedAt);
