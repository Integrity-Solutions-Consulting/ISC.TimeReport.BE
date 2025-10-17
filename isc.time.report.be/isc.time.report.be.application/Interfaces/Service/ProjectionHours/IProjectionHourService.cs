using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.ProjectionHours
{
    public interface IProjectionHourService
    {
        Task<List<ProjectionWithoutProjectResponse>> GetAllProjectionByProjectId(Guid projectionId);
        Task<CreateProjectionWithoutProjectResponse> CreateAsync(CreateProjectionWithoutProjectRequest request);
        Task<UpdateProjectionWithoutProjectResponse> UpdateAsync(UpdateProjectionWithoutProjectRequest request, Guid groupProjection, int resourceTypeId);
        Task ActivateInactiveResourceAsync(Guid groupProjection, int resourceTypeId, bool active);
        Task<byte[]> ExportProjectionWithoutProjectToExcelAsync(Guid groupProjectionId);
    }
}
