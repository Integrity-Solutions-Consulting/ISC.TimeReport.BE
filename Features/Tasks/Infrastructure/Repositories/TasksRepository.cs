namespace isc_tmr_backend.Features.Tasks.Infrastructure.Repositories;

using FluentResults;
using isc_tmr_backend.Features.Tasks.Domain;
using isc_tmr_backend.Infrastructure.Extensions;
using isc_tmr_backend.Infrastructure.Persistence;
using isc_tmr_backend.Infrastructure.Presentation;
using Microsoft.EntityFrameworkCore;

public class TasksRepository : ITaskRepository
{
    private readonly WriteDbContext _db;

    public TasksRepository(WriteDbContext db)
    {
        _db = db;
    }

    public async Task<Result<TaskItem>> AddAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        try
        {
            await _db.Tasks.AddAsync(task, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return Result.Ok(task);
        }
        catch (Exception ex)
        {
            return Result.Fail(TaskErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<TaskItem>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            TaskItem? task = await _db.Tasks.FindAsync([id], cancellationToken);
            if (task is null) return Result.Fail(TaskErrors.NotFound(id));
            return Result.Ok(task);
        }
        catch (Exception ex)
        {
            return Result.Fail(TaskErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<TaskItem>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            List<TaskItem>? tasks = await _db.Tasks
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
            return Result.Ok<IEnumerable<TaskItem>>(tasks);
        }
        catch (Exception ex)
        {
            return Result.Fail(TaskErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<PagedResult<TaskItem>>> GetPagedAsync(int page, int take, Guid? projectId = null, Guid? assigneeId = null, string? sortBy = null, bool ascending = true, string? search = null, CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<TaskItem> query = _db.Tasks.AsNoTracking();

            if (projectId.HasValue)
            {
                query = query.Where(t => t.ProjectId == projectId.Value);
            }

            if (assigneeId.HasValue)
            {
                query = query.Where(t => t.AssigneeId == assigneeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.ApplySearch(search, t => t.Title, t => t.Description);
            }

            query = query.ApplySort(sortBy, ascending);

            var (pagedQuery, totalCount) = query.ApplyPagination(page, take);

            List<TaskItem> tasks = await pagedQuery.ToListAsync(cancellationToken);

            return Result.Ok(new PagedResult<TaskItem>(tasks, totalCount));
        }
        catch (Exception ex)
        {
            return Result.Fail(TaskErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<TaskItem>>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            List<TaskItem>? tasks = await _db.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
            return Result.Ok<IEnumerable<TaskItem>>(tasks);
        }
        catch (Exception ex)
        {
            return Result.Fail(TaskErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<TaskItem>>> GetByAssigneeIdAsync(Guid assigneeId, CancellationToken cancellationToken = default)
    {
        try
        {
            List<TaskItem>? tasks = await _db.Tasks
                .AsNoTracking()
                .Where(t => t.AssigneeId == assigneeId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
            return Result.Ok<IEnumerable<TaskItem>>(tasks);
        }
        catch (Exception ex)
        {
            return Result.Fail(TaskErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<TaskItem>> UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        try
        {
            _db.Tasks.Update(task);
            await _db.SaveChangesAsync(cancellationToken);
            return Result.Ok(task);
        }
        catch (Exception ex)
        {
            return Result.Fail(TaskErrors.UpdateFailed(ex.Message));
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            TaskItem? task = await _db.Tasks.FindAsync([id], cancellationToken);
            if (task is null) return Result.Fail(TaskErrors.NotFound(id));

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(TaskErrors.DeleteFailed(ex.Message));
        }
    }
}
