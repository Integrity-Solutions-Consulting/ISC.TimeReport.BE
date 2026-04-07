namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();  // muestra los valores de los parámetros
                options.EnableDetailedErrors();        // errores más descriptivos
            }
        });

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"));

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();  // muestra los valores de los parámetros
                options.EnableDetailedErrors();        // errores más descriptivos
            }
        });

        return services;
    }
}