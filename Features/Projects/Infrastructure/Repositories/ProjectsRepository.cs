namespace isc_tmr_backend.Features.Projects.Infrastructure.Repositories;

using FluentResults;
using isc_tmr_backend.Features.Projects.Domain;
using isc_tmr_backend.Infrastructure.Extensions;
using isc_tmr_backend.Infrastructure.Persistence;
using isc_tmr_backend.Infrastructure.Presentation;
using Microsoft.EntityFrameworkCore;

public class ProjectsRepository : IProjectRepository
{
    private readonly WriteDbContext _db;

    public ProjectsRepository(WriteDbContext db)
    {
        _db = db;
    }

    public async Task<Result<Project>> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        try
        {
            await _db.Projects.AddAsync(project, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return Result.Ok(project);
        }
        catch (Exception ex)
        {
            return Result.Fail(ProjectErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<Project>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            Project? project = await _db.Projects.FindAsync([id], cancellationToken);
            if (project is null) return Result.Fail(ProjectErrors.NotFound(id));
            return Result.Ok(project);
        }
        catch (Exception ex)
        {
            return Result.Fail(ProjectErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<Project>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            List<Project>? projects = await _db.Projects
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(cancellationToken);
            return Result.Ok<IEnumerable<Project>>(projects);
        }
        catch (Exception ex)
        {
            return Result.Fail(ProjectErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<PagedResult<Project>>> GetPagedAsync(int page, int take, Guid? ownerId = null, string? sortBy = null, bool ascending = true, string? search = null, CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<Project> query = _db.Projects.AsNoTracking();

            if (ownerId.HasValue)
            {
                query = query.Where(p => p.OwnerId == ownerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.ApplySearch(search, p => p.Name, p => p.Description);
            }

            query = query.ApplySort(sortBy, ascending);

            var (pagedQuery, totalCount) = query.ApplyPagination(page, take);

            List<Project> projects = await pagedQuery.ToListAsync(cancellationToken);

            return Result.Ok(new PagedResult<Project>(projects, totalCount));
        }
        catch (Exception ex)
        {
            return Result.Fail(ProjectErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<Project>>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        try
        {
            List<Project>? projects = await _db.Projects
                .AsNoTracking()
                .Where(p => p.OwnerId == ownerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(cancellationToken);
            return Result.Ok<IEnumerable<Project>>(projects);
        }
        catch (Exception ex)
        {
            return Result.Fail(ProjectErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<Project>> UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        try
        {
            _db.Projects.Update(project);
            await _db.SaveChangesAsync(cancellationToken);
            return Result.Ok(project);
        }
        catch (Exception ex)
        {
            return Result.Fail(ProjectErrors.UpdateFailed(ex.Message));
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            Project? project = await _db.Projects.FindAsync([id], cancellationToken);
            if (project is null) return Result.Fail(ProjectErrors.NotFound(id));

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ProjectErrors.DeleteFailed(ex.Message));
        }
    }
}
