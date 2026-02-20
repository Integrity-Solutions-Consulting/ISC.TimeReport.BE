using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.application.Interfaces.Repository.Leaders
{
    public interface ILeaderRepository
    {
        Task<PagedResult<Leader>> GetAllLeadersPaginatedAsync(PaginationParams paginationParams, string? search);
        Task<Leader> GetLeaderByIDAsync(int leaderId);
        Task<Leader> CreateLeaderAsync(Leader leader);
        Task<Leader> UpdateLeaderAsync(Leader leader);
        Task<Leader> InactivateLeaderAsync(int leaderId);
        Task<Leader> ActivateLeaderAsync(int leaderId);
        Task<List<Leader>> GetActiveLeadersByProjectIdsAsync(List<int> projectIds);
        Task<Dictionary<int, Leader>> GetLeadersByProjectIdsDictionaryAsync(List<int> projectIds);
        Task SaveLeadersAsync(List<Leader> leaders);

    }
}
