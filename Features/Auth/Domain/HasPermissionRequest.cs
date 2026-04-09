namespace isc_tmr_backend.Features.Auth.Domain;

public record HasPermissionRequest(
    Dictionary<string, string[]>? Permission = null,
    Dictionary<string, string[]>? Permissions = null
);
