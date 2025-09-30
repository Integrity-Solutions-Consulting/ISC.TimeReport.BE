using isc.time.report.be.domain.Entity.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.Projections
{
    public interface IProjectionHourProjectRepository
    {
        Task<List<ProjectionHoursProjectResponse>> GetAllProjectionsAsync(int projectId);
        Task<ProjectionHourProject> CreateProjectionAsync(ProjectionHourProject entity);
        Task<ProjectionHourProject> UpdateResourceAssignedToProjectionAsync(ProjectionHourProject entity, int projectid, int id);
        Task<ProjectionHourProject?> GetResourceByProjectionIdAsync(int projectId, int id);
    }
}
