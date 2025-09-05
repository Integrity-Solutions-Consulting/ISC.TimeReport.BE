using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Request.Projects;
using isc.time.report.be.domain.Models.Response.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.Projects
{
    public interface IProjectService
    {
        Task<CreateProjectResponse> CreateProject(CreateProjectRequest projectRequest);
        Task<PagedResult<GetAllProjectsResponse>> GetAllProjectsPaginated(PaginationParams paginationParams, string? search);
        Task<PagedResult<GetAllProjectsResponse>> GetAllProjectsByEmployeeIDPaginated(PaginationParams paginationParams, string? search, int employeeId);
        Task<GetProjectByIDResponse> GetProjectByID(int projectID);
        Task<UpdateProjectResponse> UpdateProject(int projectId, UpdateProjectRequest projectParaUpdate);
        Task<ActiveInactiveProjectResponse> InactiveProject(int projectId);
        Task<ActiveInactiveProjectResponse> ActiveProject(int projectId);
        Task AssignEmployeesToProject(AssignEmployeesToProjectRequest request);
        Task<GetProjectDetailByIDResponse?> GetProjectDetailByID(int projectID);
        Task<List<GetProjectsByEmployeeIDResponse>> GetProjectsByEmployeeIdAsync(int employeeId);
        Task<List<CreateDtoToExcelProject>> GetProjectsForExcelAsync();
        Task<byte[]> GenerateProjectsExcelAsync();
    }
}
