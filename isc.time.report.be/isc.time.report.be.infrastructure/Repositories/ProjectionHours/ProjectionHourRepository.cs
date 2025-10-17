using isc.time.report.be.application.Interfaces.Repository.ProjectionHours;
using isc.time.report.be.application.Interfaces.Repository.Projections;
using isc.time.report.be.domain.Entity.ProjectionHours;
using isc.time.report.be.domain.Entity.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using isc.time.report.be.infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.ProjectionHours
{
    public class ProjectionHourRepository : IProjectionHoursRepository
    {
        private readonly DBContext _dbContext;
        public ProjectionHourRepository(DBContext dBContext) {
            _dbContext = dBContext;
        }

        public async Task<List<ProjectionWithoutProjectResponse>> GetAllProjectionsWithoutProjectAsync(Guid groupProjection)
        {
            var parameters = new[] {
                new SqlParameter("@GroupProjection", groupProjection)
            };
            return await _dbContext.Set<ProjectionWithoutProjectResponse>()
                .FromSqlRaw("EXEC dbo.sp_GetProjectionHoursByGroup @GroupProjection", parameters)
                .ToListAsync();
        }


        public async Task<ProjectionHour> CreateProjectionWithoutProjectAsync(ProjectionHour entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.CreationUser = "SYSTEM";
            entity.Status = true;
            await _dbContext.ProjectionHour.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<ProjectionHour> UpdateResourceAssignedToProjectionAsync(ProjectionHour entity)
        {

            // 2️⃣ Marcar toda la entidad como modificada
            _dbContext.Entry(entity).State = EntityState.Modified;

            // 3️⃣ Guardar los cambios
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<int> ActiveInactiveResourceOfProjectionWithoutProjectAsync(Guid projectionId, int resourceTypeId, bool status)
        {
            return await _dbContext.ProjectionHour
                .Where(r => r.GroupProjection == projectionId && r.ResourceTypeId == resourceTypeId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(r => r.Status, status)
                );
        }

        public async Task<ProjectionHour?> GetResourceByProjectionIdAsync(Guid projectionId, int id)
        {
            var resource = await _dbContext.ProjectionHour.FirstOrDefaultAsync(r => r.GroupProjection == projectionId && r.ResourceTypeId == id);
            return resource;
        }
    }
}
