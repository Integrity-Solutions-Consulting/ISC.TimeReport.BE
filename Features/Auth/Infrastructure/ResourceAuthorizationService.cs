namespace isc_tmr_backend.Features.Auth.Infrastructure;

using System.Net.Http.Json;
using isc_tmr_backend.Features.Auth.Domain;
using Microsoft.Extensions.Options;

public class ResourceAuthorizationService : IResourceAuthorizationService
{
    private readonly HttpClient _httpClient;
    private readonly BettersAuthOptions _options;
    private readonly ILogger<ResourceAuthorizationService> _logger;

    public ResourceAuthorizationService(
        HttpClient httpClient,
        IOptions<BettersAuthOptions> options,
        ILogger<ResourceAuthorizationService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<AuthorizationResult> HasPermissionAsync(
        Dictionary<string, string[]> permission,
        CancellationToken cancellationToken = default)
    {
        return await CheckPermissionsAsync(permission, null, cancellationToken);
    }

    public async Task<AuthorizationResult> HasPermissionsAsync(
        Dictionary<string, string[]> permissions,
        CancellationToken cancellationToken = default)
    {
        return await CheckPermissionsAsync(null, permissions, cancellationToken);
    }

    private async Task<AuthorizationResult> CheckPermissionsAsync(
        Dictionary<string, string[]>? permission,
        Dictionary<string, string[]>? permissions,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new HasPermissionRequest(permission, permissions);
            var url = $"{_options.BaseUrl.TrimEnd('/')}{_options.HasPermissionEndpoint}";

            _logger.LogInformation("Calling better-auth at {Url} with request: {@Request}", url, request);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HasPermissionResponse>(cancellationToken);
                
                if (result is not null)
                {
                    _logger.LogInformation("Authorization result: {Granted}, Reason: {Reason}", result.Granted, result.Reason);
                    return new AuthorizationResult(result.Granted, result.Reason);
                }
            }

            _logger.LogWarning("Authorization check failed with status: {StatusCode}", response.StatusCode);
            return new AuthorizationResult(false, $"Authorization service returned {response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling better-auth service");
            return new AuthorizationResult(false, $"Error calling authorization service: {ex.Message}");
        }
    }
}
