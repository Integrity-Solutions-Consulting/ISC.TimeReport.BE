namespace isc_tmr_backend.Features.Notifications.Domain;

using FluentResults;

public interface INotificationRepository
{
    // Escritura — usado por CommandHandlers
    Task<Result<Notification>> AddAsync(Notification notification, CancellationToken cancellationToken = default);

    // Lecturas — usadas por QueryHandlers
    Task<Result<Notification>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Notification>>> GetAllAsync(CancellationToken cancellationToken = default);
}