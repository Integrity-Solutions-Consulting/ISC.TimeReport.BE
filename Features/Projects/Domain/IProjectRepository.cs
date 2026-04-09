namespace isc_tmr_backend.Features.Projects.Domain;

using FluentResults;
using isc_tmr_backend.Infrastructure.Presentation;

public interface IProjectRepository
{
    Task<Result<Project>> AddAsync(Project project, CancellationToken cancellationToken = default);
    Task<Result<Project>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Project>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PagedResult<Project>>> GetPagedAsync(int page, int take, Guid? ownerId = null, string? sortBy = null, bool ascending = true, string? search = null, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Project>>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<Result<Project>> UpdateAsync(Project project, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
