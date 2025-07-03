using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.Leaders
{
    public interface ILeaderRepository
    {
        Task<PagedResult<Leader>> GetAllLeadersPaginatedAsync(PaginationParams paginationParams, string? search);
        Task<Leader> GetLeaderByIDAsync(int leaderId);
        Task<Leader> CreateLeaderAsync(Leader leader);
        Task<Leader> CreateLeaderWithPersonAsync(Leader leader);
        Task<Leader> UpdateLeaderAsync(Leader leader);
        Task<Leader> UpdateLeaderWithPersonAsync(Leader leader);
        Task<Leader> InactivateLeaderAsync(int leaderId);
        Task<Leader> ActivateLeaderAsync(int leaderId);
    }
}
