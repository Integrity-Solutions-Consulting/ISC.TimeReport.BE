namespace isc_tmr_backend.Features.Tasks.Domain;

using FluentResults;
using isc_tmr_backend.Infrastructure.Presentation;

public interface ITaskRepository
{
    Task<Result<TaskItem>> AddAsync(TaskItem task, CancellationToken cancellationToken = default);
    Task<Result<TaskItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<TaskItem>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TaskItem>>> GetPagedAsync(int page, int take, Guid? projectId = null, Guid? assigneeId = null, string? sortBy = null, bool ascending = true, string? search = null, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<TaskItem>>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<TaskItem>>> GetByAssigneeIdAsync(Guid assigneeId, CancellationToken cancellationToken = default);
    Task<Result<TaskItem>> UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
