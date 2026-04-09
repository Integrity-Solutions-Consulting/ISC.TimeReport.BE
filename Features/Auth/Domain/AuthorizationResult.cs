namespace isc_tmr_backend.Features.Auth.Domain;

public record AuthorizationResult(
    bool IsAuthorized,
    string? Reason = null
);
