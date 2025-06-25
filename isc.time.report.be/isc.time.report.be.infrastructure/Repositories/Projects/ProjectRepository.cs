using isc.time.report.be.application.Interfaces.Repository.Projects;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DBContext _dbContext;

        public ProjectRepository(DBContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Project>> GetAllProjectsPaginatedAsync(PaginationParams paginationParams)
        {
            var query = _dbContext.Projects.AsQueryable();
            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<Project> GetProjectByIDAsync(int projectId)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            return project;
        }
        public async Task<Project> CreateProject(Project project)
        {
            project.CreationDate = DateTime.Now;
            project.ModificationDate = null;
            project.Status = true;

            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
            project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == project.Id && p.Status == true);
            return project;
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            project.ModificationDate = DateTime.Now;

            _dbContext.Entry(project).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<Project> InactivateProjectAsync(int projectId)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new InvalidOperationException($"El proyecto con ID {projectId} no existe.");

            project.Status = false;
            project.ModificationDate = DateTime.Now;

            _dbContext.Entry(project).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<Project> ActivateProjectAsync(int projectId)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new InvalidOperationException($"El proyecto con ID {projectId} no existe.");

            project.Status = true;
            project.ModificationDate = DateTime.Now;

            _dbContext.Entry(project).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<List<EmployeeProject>> GetByProjectIdAsync(int projectId)
        {
            return await _dbContext.EmployeeProjects
                .Where(ep => ep.ProjectID == projectId)
                .ToListAsync();
        }

        public async Task SaveAssignmentsAsync(List<EmployeeProject> employeeProjects)
        {
            foreach (var ep in employeeProjects)
            {
                if (ep.Id == 0)
                {
                    await _dbContext.EmployeeProjects.AddAsync(ep);
                }
                else
                {
                    _dbContext.EmployeeProjects.Update(ep);
                }
            }
            await _dbContext.SaveChangesAsync();
        }

    }
}
