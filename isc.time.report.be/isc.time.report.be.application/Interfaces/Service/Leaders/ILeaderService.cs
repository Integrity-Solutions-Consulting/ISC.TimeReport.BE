using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;

namespace isc.time.report.be.application.Interfaces.Service.Leaders
{
    public interface ILeaderService
    {
        Task<PagedResult<GetLeaderDetailsResponse>> GetAllLeadersPaginated(PaginationParams paginationParams, string? search);
        Task<GetLeaderDetailsResponse> GetLeaderByID(int leaderId);
        Task<CreateLeaderResponse> CreateLeader(CreateLeaderRequest request);
        Task<UpdateLeaderResponse> UpdateLeader(int leaderId, UpdateLeaderRequest request);
        Task<ActivateInactivateLeaderResponse> InactivateLeader(int leaderId);
        Task<ActivateInactivateLeaderResponse> ActivateLeader(int leaderId);
        Task<AssignLeaderToProjectResponse> AssignLeaderToProject(AssignLeaderToProjectRequest request);
    }
}
