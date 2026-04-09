namespace isc_tmr_backend.Features.Tasks.Application.Queries;

using FluentResults;
using MediatR;

public record GetTaskByIdQuery(Guid Id) : IRequest<Result<GetTaskResponse>>;
