using isc.time.report.be.domain.Entity.ProjectionHours;
using isc.time.report.be.domain.Models.Response.Projections;

namespace isc.time.report.be.application.Interfaces.Repository.ProjectionHours
{
    public interface IProjectionHoursRepository
    {
        Task<List<ProjectionWithoutProjectResponse>> GetProjectionsByGuidWithoutProjectAsync(Guid groupProjection);
        Task<ProjectionHour> CreateProjectionWithoutProjectAsync(ProjectionHour entity);
        Task<ProjectionHour> UpdateResourceAssignedToProjectionAsync(ProjectionHour entity);
        Task<int> ActiveInactiveResourceOfProjectionWithoutProjectAsync(Guid projectionId, int resourceTypeId, bool status);
        Task<ProjectionHour?> GetResourceByProjectionIdAsync(Guid projectionId, int id);
        Task<List<Guid>> GetAllGroupProjectionsAsync();

    }
}
