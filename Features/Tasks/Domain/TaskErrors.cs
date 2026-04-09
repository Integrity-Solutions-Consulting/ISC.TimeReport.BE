namespace isc_tmr_backend.Features.Tasks.Domain;

using FluentResults;

public static class TaskErrors
{
    public static IError NotFound(Guid id) => new Error($"Task with id '{id}' was not found.");

    public static IError ProjectNotFound(Guid projectId) => new Error($"Project with id '{projectId}' was not found.");

    public static IError CreateFailed(string reason) => new Error($"Failed to create task: {reason}");

    public static IError UpdateFailed(string reason) => new Error($"Failed to update task: {reason}");

    public static IError DeleteFailed(string reason) => new Error($"Failed to delete task: {reason}");
}
