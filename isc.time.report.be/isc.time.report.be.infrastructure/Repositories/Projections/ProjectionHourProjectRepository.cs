using Azure.Core;
using DocumentFormat.OpenXml.InkML;
using isc.time.report.be.application.Interfaces.Repository.Projections;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Projections;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Exceptions;
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
        public async Task<ProjectionHourProject> CreateProjectionAsync(ProjectionHourProject entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.CreationUser = "SYSTEM";
            entity.Status = true;
            await _dbContext.ProjectionHoursProjects.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<ProjectionHourProject> UpdateResourceAssignedToProjectionAsync(ProjectionHourProject entity, int projectid, int id)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<int> ActiveInactiveResourceOfProjectionAsync(int projectId, int resourceTypeId, bool status)
        {
            return await _dbContext.ProjectionHoursProjects
                .Where(r => r.ProjectId == projectId && r.ResourceTypeId == resourceTypeId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(r => r.Status, status)
                );
        }

        public async Task<ProjectionHourProject?> GetResourceByProjectionIdAsync(int projectId, int id)
        {
            if (id <= 0)
            {
                throw new ClientFaultException("El ID del Daily no puede ser negativo");
            }
            var resource = await _dbContext.ProjectionHoursProjects.FindAsync(id);
            return resource;
        }
    }


}
