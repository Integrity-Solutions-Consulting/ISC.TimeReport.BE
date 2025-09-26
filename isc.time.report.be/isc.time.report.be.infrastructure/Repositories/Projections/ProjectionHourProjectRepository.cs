using isc.time.report.be.application.Interfaces.Repository.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
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
        private readonly DbContext _dbContext;
        public ProjectionHourProjectRepository(DbContext dbContext) { 
            _dbContext = dbContext;
        
        }
        //public async Task<List<ProjectionHoursProjectResponse>> GetAllProjectionsAsync(int IDRecurso, string TipoRecurso, string NombreRecurso, decimal CostoPorHora)
        //{

        //}
    }
}
