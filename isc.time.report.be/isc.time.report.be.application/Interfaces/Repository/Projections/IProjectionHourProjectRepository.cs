using isc.time.report.be.domain.Entity.Projections;
using isc.time.report.be.domain.Models.Response.Projections;

namespace isc.time.report.be.application.Interfaces.Repository.Projections
{
    public interface IProjectionHourProjectRepository
    {
        Task<List<ProjectionHoursProjectResponse>> GetAllProjectionsAsync(int projectId);
        Task<ProjectionHourProject> CreateProjectionAsync(ProjectionHourProject entity);
        Task<ProjectionHourProject> UpdateResourceAssignedToProjectionAsync(ProjectionHourProject entity, int projectid, int id);
        Task<ProjectionHourProject?> GetResourceByProjectionIdAsync(int projectId, int id);
        Task<int> ActiveInactiveResourceOfProjectionAsync(int projectId, int resourceTypeId, bool status);
    }
}
