namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Features.Users.Application.Commands;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRConfig(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly));

        return services;
    }
}
