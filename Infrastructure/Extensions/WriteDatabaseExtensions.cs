namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class WriteDatabaseExtensions
{
    public static IServiceCollection AddWriteDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WriteDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("WriteDbConnection"));

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        return services;
    }
}
