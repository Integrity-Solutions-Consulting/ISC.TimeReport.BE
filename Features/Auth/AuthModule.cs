namespace isc_tmr_backend.Features.Auth;

using isc_tmr_backend.Features.Auth.Domain;
using isc_tmr_backend.Features.Auth.Endpoint;
using isc_tmr_backend.Features.Auth.Infrastructure;

public static class AuthModule
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BettersAuthOptions>(configuration.GetSection("BettersAuth"));
        
        services.AddHttpClient<IResourceAuthorizationService, ResourceAuthorizationService>();

        return services;
    }

    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        AuthEndpoint.MapAuthEndpoints(group);
        return group;
    }

    public static WebApplication UseAuthMiddleware(this WebApplication app)
    {
        app.UseStubAuthorization();
        return app;
    }
}
