namespace isc_tmr_backend.Features.Users.Application.Queries;

using FluentResults;
using MediatR;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<GetUserResponse>>;
