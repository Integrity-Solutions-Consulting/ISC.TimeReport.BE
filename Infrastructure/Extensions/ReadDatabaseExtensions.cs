namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class ReadDatabaseExtensions
{
    public static IServiceCollection AddReadDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReadDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("ReadDbConnection"));

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        return services;
    }
}
