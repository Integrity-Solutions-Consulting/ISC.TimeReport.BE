namespace isc_tmr_backend.Features.Users.Domain;

using FluentResults;
using isc_tmr_backend.Infrastructure.Presentation;

public interface IUserRepository
{
    Task<Result<User>> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PagedResult<User>>> GetPagedAsync(int page, int take, string? sortBy = null, bool ascending = true, string? search = null, CancellationToken cancellationToken = default);
    Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
