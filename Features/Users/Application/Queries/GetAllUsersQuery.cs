namespace isc_tmr_backend.Features.Users.Application.Queries;

using FluentResults;
using MediatR;

public record GetAllUsersQuery : IRequest<Result<IEnumerable<GetUserResponse>>>;

public record GetUserResponse(Guid Id, string Email, string DisplayName, DateTime CreatedAt);
