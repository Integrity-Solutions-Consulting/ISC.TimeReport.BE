namespace isc_tmr_backend.Features.Activities.Application.Queries;

using FluentResults;
using MediatR;

public record GetAllActivitiesQuery : IRequest<Result<IEnumerable<string>>>;