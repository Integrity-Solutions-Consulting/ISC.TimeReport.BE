namespace isc_tmr_backend.Infrastructure.Persistence;

using isc_tmr_backend.Features.Users.Domain;
using isc_tmr_backend.Features.Projects.Domain;
using isc_tmr_backend.Features.Tasks.Domain;
using Microsoft.EntityFrameworkCore;

public class WriteDbContext(DbContextOptions<WriteDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);
    }
}
