namespace isc_tmr_backend.Features.Users.Domain;

using FluentResults;

public static class UserErrors
{
    public static IError NotFound(Guid id) => new Error($"User with id '{id}' was not found.");

    public static IError AlreadyExists(Guid id) => new Error($"User with id '{id}' already exists.");

    public static IError CreateFailed(string reason) => new Error($"Failed to create user: {reason}");

    public static IError UpdateFailed(string reason) => new Error($"Failed to update user: {reason}");

    public static IError DeleteFailed(string reason) => new Error($"Failed to delete user: {reason}");
}
