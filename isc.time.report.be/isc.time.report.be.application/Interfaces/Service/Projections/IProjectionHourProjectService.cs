using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;

namespace isc.time.report.be.application.Interfaces.Service.Projections
{
    public interface IProjectionHourProjectService
    {
        Task<List<ProjectionHoursProjectResponse>> GetAllProjectionByProjectId(int projectId);
        Task<CreateProjectionHoursProjectResponse> CreateAsync(ProjectionHoursProjectRequest request, int projectId);
        Task<UpdateProjectionHoursProjectResponse> UpdateAsync(UpdateProjectionHoursProjectRequest request, int resourceTypeId, int projectId);
        Task ActivateInactiveResourceAsync(int projectId, int resourceTypeId, bool active);
        Task<byte[]> ExportProjectionToExcelAsync(int projectId);

    }
}
