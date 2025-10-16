using isc.time.report.be.domain.Entity.Projections;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.Projections
{
    public interface IProjectionHourProjectService  
    {
        Task<List<ProjectionHoursProjectResponse>> GetAllProjectionByProjectId(int projectId);
        Task<CreateProjectionHoursProjectResponse> CreateAsync(ProjectionHoursProjectRequest request, int projectId);
        Task<UpdateProjectionHoursProjectResponse> UpdateAsync(UpdateProjectionHoursProjectRequest request, int resourceTypeId, int projectId);
        Task ActivateInactiveResourceAsync(int projectId, int resourceTypeId, bool active);
        Task<byte[]> ExportProjectionToExcelAsync(int projectId);
        Task<ProjectionWithoutProjectResponse> GetProjectionWithoutProjectByIdAsync(GetProjectionWithoutProjectByIdRequest request);
        Task<CreateProjectionWithoutProjectResponse> CreateProjectionWithoutProjectAsync(CreateProjectionWithoutProjectRequest request);
        Task<UpdateProjectionWithoutProjectResponse> UpdateProjectionWithooutProjectAsync(UpdateProjectionWithoutProjectRequest request);
    }
}
