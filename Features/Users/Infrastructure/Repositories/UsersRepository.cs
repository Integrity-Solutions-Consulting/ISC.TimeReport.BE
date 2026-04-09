namespace isc_tmr_backend.Features.Users.Infrastructure.Repositories;

using FluentResults;
using isc_tmr_backend.Features.Users.Domain;
using isc_tmr_backend.Infrastructure.Extensions;
using isc_tmr_backend.Infrastructure.Persistence;
using isc_tmr_backend.Infrastructure.Presentation;
using Microsoft.EntityFrameworkCore;

public class UsersRepository : IUserRepository
{
    private readonly WriteDbContext _db;

    public UsersRepository(WriteDbContext db)
    {
        _db = db;
    }

    public async Task<Result<User>> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            await _db.Users.AddAsync(user, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            return Result.Fail(UserErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            User? user = await _db.Users.FindAsync([id], cancellationToken);
            if (user is null) return Result.Fail(UserErrors.NotFound(id));
            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            return Result.Fail(UserErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            List<User>? users = await _db.Users
                .AsNoTracking()
                .OrderBy(u => u.DisplayName)
                .ToListAsync(cancellationToken);
            return Result.Ok<IEnumerable<User>>(users);
        }
        catch (Exception ex)
        {
            return Result.Fail(UserErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<PagedResult<User>>> GetPagedAsync(int page, int take, string? sortBy = null, bool ascending = true, string? search = null, CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<User> query = _db.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.ApplySearch(search, u => u.Email, u => u.DisplayName);
            }

            query = query.ApplySort(sortBy, ascending);

            var (pagedQuery, totalCount) = query.ApplyPagination(page, take);

            List<User> users = await pagedQuery.ToListAsync(cancellationToken);

            return Result.Ok(new PagedResult<User>(users, totalCount));
        }
        catch (Exception ex)
        {
            return Result.Fail(UserErrors.CreateFailed(ex.Message));
        }
    }

    public async Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync(cancellationToken);
            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            return Result.Fail(UserErrors.UpdateFailed(ex.Message));
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            User? user = await _db.Users.FindAsync([id], cancellationToken);
            if (user is null) return Result.Fail(UserErrors.NotFound(id));

            _db.Users.Remove(user);
            await _db.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(UserErrors.DeleteFailed(ex.Message));
        }
    }
}
