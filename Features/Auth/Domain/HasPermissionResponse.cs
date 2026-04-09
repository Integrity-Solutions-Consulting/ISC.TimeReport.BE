namespace isc_tmr_backend.Features.Auth.Domain;

public record HasPermissionResponse(
    bool Granted,
    string? Reason = null
);
