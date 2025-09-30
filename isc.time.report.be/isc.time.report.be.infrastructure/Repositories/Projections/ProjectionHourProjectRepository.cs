using isc.time.report.be.application.Interfaces.Repository.Projections;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Models.Response.Projections;
using isc.time.report.be.infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Projections
{
    public class ProjectionHourProjectRepository : IProjectionHourProjectRepository
    {
        private readonly DBContext _dbContext;

        public ProjectionHourProjectRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<ProjectionHoursProjectResponse>> GetAllProjectionsAsync(int projectId)
        {
            var parameters = new[] {
                new SqlParameter("@ProjectId", projectId)
            };
            return await _dbContext.Set<ProjectionHoursProjectResponse>()
                .FromSqlRaw("EXEC dbo.sp_ProyeccionHorasPorProyectoGOOOD @ProjectID", parameters)
                .ToListAsync();
        }
    }


}
