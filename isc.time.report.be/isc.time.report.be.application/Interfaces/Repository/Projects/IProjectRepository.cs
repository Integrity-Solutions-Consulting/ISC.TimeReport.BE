using isc.time.report.be.domain.Entity.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.Projects
{
    public interface IProjectRepository
    {
        Task<Project> CreateProject(Project project);
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIDAsync(int projectId);
        Task<Project> UpdateProjectAsync(Project project);
        Task<Project> InactivateProjectAsync(int projectId);
        Task<Project> ActivateProjectAsync(int projectId);

    }
}
