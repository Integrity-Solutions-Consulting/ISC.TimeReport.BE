namespace isc_tmr_backend.Features.Projects.Domain;

using FluentResults;

public static class ProjectErrors
{
    public static IError NotFound(Guid id) => new Error($"Project with id '{id}' was not found.");

    public static IError OwnerNotFound(Guid ownerId) => new Error($"Owner with id '{ownerId}' was not found.");

    public static IError CreateFailed(string reason) => new Error($"Failed to create project: {reason}");

    public static IError UpdateFailed(string reason) => new Error($"Failed to update project: {reason}");

    public static IError DeleteFailed(string reason) => new Error($"Failed to delete project: {reason}");
}
