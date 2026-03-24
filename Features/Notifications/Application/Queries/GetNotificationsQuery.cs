namespace isc_tmr_backend.Features.Notifications.Application.Queries;

using FluentResults;
using MediatR;

public record GetNotificationsQuery : IRequest<Result<IEnumerable<GetNotificationResponse>>>;

public record GetNotificationRequest(Guid Id, string To, string Subject, string Channel, DateTime CreatedAt);

public record GetNotificationResponse(Guid Id, string To, string Subject, string Channel, DateTime CreatedAt);