namespace isc_tmr_backend.Features.Auth.Infrastructure;

public class BettersAuthOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string HasPermissionEndpoint { get; set; } = "/has-permission";
}
