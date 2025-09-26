using isc.time.report.be.application.Interfaces.Repository.Projections;
using isc.time.report.be.application.Interfaces.Service.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.Projections
{
    public class ProjectionHourProjectService : IProjectionHourProjectService
    {
        private readonly IProjectionHourProjectRepository _projectionHourProjectRepository ;
        public ProjectionHourProjectService(IProjectionHourProjectRepository projectionHourProjectRepository) { 

            _projectionHourProjectRepository = projectionHourProjectRepository ;

        }

        //public async Task<List<ProjectionHoursProjectResponse>> GetAllProjections()
        //{

        //}
    }
}
