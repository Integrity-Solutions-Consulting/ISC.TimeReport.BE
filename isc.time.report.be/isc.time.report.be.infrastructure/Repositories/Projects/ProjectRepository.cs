using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.infrastructure.Database;

namespace isc.time.report.be.infrastructure.Repositories.Projects
{
    public class ProjectRepository
    {
        private readonly DBContext _dBContext;

        public ProjectRepository(DBContext dbContext) 
        { 
            _dBContext = dbContext;
        }
        public async Task<Project> CreateProject(Project project)
        {
            project.CreationDate = DateTime.Now;
            project.CreatedAt = DateTime.Now;
            project.UpdatedAt = null;
            project.Status = "Activo";
            //await _dBContext.Projects.AddAsync(project);
            await _dBContext.SaveChangesAsync();
            return project;
        }
    }
}
