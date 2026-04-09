namespace isc_tmr_backend.Features.Auth.Domain;

public interface IResourceAuthorizationService
{
    Task<AuthorizationResult> HasPermissionAsync(
        Dictionary<string, string[]> permission,
        CancellationToken cancellationToken = default);

    Task<AuthorizationResult> HasPermissionsAsync(
        Dictionary<string, string[]> permissions,
        CancellationToken cancellationToken = default);
}
