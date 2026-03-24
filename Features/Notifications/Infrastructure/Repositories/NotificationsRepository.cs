namespace isc_tmr_backend.Features.Notifications.Infrastructure.Repositories;

using FluentResults;
using isc_tmr_backend.Features.Notifications.Domain;
using isc_tmr_backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class NotificationsRepository(AppDbContext dbContext) : INotificationRepository
{
    public async Task<Result<Notification>> AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Notifications.AddAsync(notification, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(notification);
        }
        catch (Exception ex)
        {
            return Result.Fail(NotificationErrors.SendFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<Notification>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // AsNoTracking le dice a EF Core que no rastree cambios en estas entidades
            // porque solo vamos a leer, no a modificar
            // mejora el rendimiento porque EF Core no guarda snapshots de las entidades
            List<Notification>? notifications = await dbContext.Notifications
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // el cast explícito a IEnumerable<Notification> es necesario
            // porque Result.Ok infiere el tipo del valor que le pasas
            // y ToListAsync retorna List<Notification>, no IEnumerable<Notification>
            return Result.Ok<IEnumerable<Notification>>(notifications);
        }
        catch (Exception ex)
        {
            return Result.Fail(NotificationErrors.SendFailed(ex.Message));
        }
    }

    public async Task<Result<Notification>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // FindAsync es más eficiente que FirstOrDefaultAsync para búsquedas por PK
            // porque primero revisa el cache del DbContext antes de ir a la DB
            Notification? notification = await dbContext.Notifications.FindAsync([id], cancellationToken);

            // en lugar de retornar null retornamos un error tipado
            // el Endpoint sabrá que esto debe traducirse a un 404
            if (notification is null) return Result.Fail(NotificationErrors.NotFound(id));

            return Result.Ok(notification);
        }
        catch (Exception ex)
        {
            return Result.Fail(NotificationErrors.SendFailed(ex.Message));
        }
    }
}