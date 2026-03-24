namespace isc_tmr_backend.Features.Notifications.Application.Commands;

using FluentResults;
using isc_tmr_backend.Features.Notifications.Domain;
using MediatR;


public record SendNotificationCommand(SendNotificationRequest Request) : IRequest<Result<SendNotificationResponse>>;
public record SendNotificationRequest(string To, string Subject, string Body, NotificationChannel Channel);
public record SendNotificationResponse(Guid NotificationId, bool Success, string Message);