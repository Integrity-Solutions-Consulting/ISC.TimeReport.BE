namespace isc_tmr_backend.Features.Todo.Application.Queries;

using FluentResults;
using MediatR;

public record GetAllTodoQuery : IRequest<Result<IEnumerable<string>>>;