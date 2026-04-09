namespace isc_tmr_backend.Features.Auth.Endpoint;

using FluentResults;
using isc_tmr_backend.Features.Auth.Domain;
using isc_tmr_backend.Infrastructure.Extensions;
using isc_tmr_backend.Infrastructure.Presentation;
using System.Net;

public static class AuthEndpoint
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/auth/test", TestAuthorization)
            .WithName("TestAuthorization")
            .WithTags("Auth")
            .Produces<ResponseWithMetadata<AuthorizationResult>>(200)
            .Produces(401)
            .Produces(403);

        return group;
    }

    private static async Task<IResult> TestAuthorization(
        string resource,
        string permissions,
        IResourceAuthorizationService authService,
        CancellationToken cancellationToken)
    {
        var permissionsDict = ParsePermissions(permissions, resource);
        
        var result = await authService.HasPermissionsAsync(permissionsDict, cancellationToken);

        if (result.IsAuthorized)
        {
            var successResult = Result.Ok(result);
            return successResult.ToSuccessResponse($"Access granted for {resource}", HttpStatusCode.OK);
        }

        var errorResult = Result.Fail(result.Reason ?? "Access denied");
        return errorResult.ToErrorResponse(HttpStatusCode.Forbidden);
    }

    private static Dictionary<string, string[]> ParsePermissions(string permissions, string resource)
    {
        var result = new Dictionary<string, string[]>();
        var permissionList = permissions.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .ToArray();

        result[resource] = permissionList;

        return result;
    }
}
