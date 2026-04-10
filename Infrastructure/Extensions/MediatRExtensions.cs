namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Features.Users.Application.Commands;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            // Obtiene la clave de licencia desde la configuración
            config.LicenseKey = configuration.GetValue<string>("MediatR:LicenseKey") ?? string.Empty;

            // Registra los servicios desde el ensamblado del manejador de comandos
            config.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly);
        });

        return services;
    }
}
