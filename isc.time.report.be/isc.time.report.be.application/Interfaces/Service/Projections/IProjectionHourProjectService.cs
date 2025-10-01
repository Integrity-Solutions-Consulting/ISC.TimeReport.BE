using isc.time.report.be.domain.Entity.Projections;
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
        Task<ProjectionHoursProjectRequest> CreateAsync(ProjectionHoursProjectRequest request);
        Task<UpdateProjectionHoursProjectRequest> UpdateAsync(UpdateProjectionHoursProjectRequest request, int resourceTypeId, int projectId);
    }
}
