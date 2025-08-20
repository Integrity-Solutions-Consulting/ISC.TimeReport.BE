using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.Leaders
{
    public interface ILeaderService
    {
        Task<PagedResult<GetLeaderDetailsResponse>> GetAllLeadersPaginated(PaginationParams paginationParams, string? search);
        Task<GetLeaderDetailsResponse> GetLeaderByID(int leaderId);
        Task<CreateLeaderResponse> CreateLeaderWithPersonID(CreateLeaderWithPersonIDRequest request);
        Task<CreateLeaderResponse> CreateLeaderWithPerson(CreateLeaderWithPersonOBJRequest request);
        Task<UpdateLeaderResponse> UpdateLeader(int leaderId, UpdateLeaderWithPersonIDRequest request);
        Task<UpdateLeaderResponse> UpdateLeaderWithPerson(int leaderId, UpdateLeaderWithPersonOBJRequest request);
        Task<ActivateInactivateLeaderResponse> InactivateLeader(int leaderId);
        Task<ActivateInactivateLeaderResponse> ActivateLeader(int leaderId);
        Task<List<GetAllLeaderProjectByPersonIdResponse>> GetAllLeadersRegisterGrouped();
        Task<GetAllLeaderProjectByPersonIdResponse?> GetLeadershipByPersonId(int personId);
    }
}
