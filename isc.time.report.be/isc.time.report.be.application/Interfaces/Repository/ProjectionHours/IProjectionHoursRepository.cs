using isc.time.report.be.domain.Entity.ProjectionHours;
using isc.time.report.be.domain.Models.Response.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.ProjectionHours
{
    public interface IProjectionHoursRepository
    {
        Task<List<ProjectionWithoutProjectResponse>> GetAllProjectionsWithoutProjectAsync(Guid groupProjection);
        Task<ProjectionHour> CreateProjectionWithoutProjectAsync(ProjectionHour entity);
        Task<ProjectionHour> UpdateResourceAssignedToProjectionAsync(ProjectionHour entity);
        Task<int> ActiveInactiveResourceOfProjectionWithoutProjectAsync(Guid projectionId, int resourceTypeId, bool status);
        Task<ProjectionHour?> GetResourceByProjectionIdAsync(Guid projectionId, int id);

    }
}
