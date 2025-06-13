using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Projects;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DBContext _dbContext;

        public ProjectRepository(DBContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _dbContext.Projects.ToListAsync();
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

    }
}
