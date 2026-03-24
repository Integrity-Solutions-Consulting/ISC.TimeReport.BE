namespace isc_tmr_backend.Infrastructure.Persistence;


using isc_tmr_backend.Features.Notifications.Domain;
using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Escanea el assembly buscando clases que implementen
        // IEntityTypeConfiguration<T> y las aplica automáticamente
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}