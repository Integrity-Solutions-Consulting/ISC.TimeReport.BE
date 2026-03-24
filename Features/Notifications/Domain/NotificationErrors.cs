namespace isc_tmr_backend.Features.Notifications.Domain;

using FluentResults;

public static class NotificationErrors
{

    public static IError NotFound(Guid id) => new Error($"Notification with id '{id}' was not found.");

    public static IError AlreadyExists(Guid id) => new Error($"Notification with id '{id}' already exists.");

    public static IError SendFailed(string reason) => new Error($"Failed to send notification: {reason}");
}