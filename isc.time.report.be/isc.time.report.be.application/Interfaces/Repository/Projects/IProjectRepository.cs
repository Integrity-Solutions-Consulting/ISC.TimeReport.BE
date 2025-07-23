using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
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
        Task<PagedResult<Project>> GetAllProjectsPaginatedAsync(PaginationParams paginationParams, string? search);
        Task<PagedResult<Project>> GetAssignedProjectsForEmployeeAsync(PaginationParams paginationParams, string? search, int employeeId);
        Task<Project> GetProjectByIDAsync(int projectId);
        Task<Project> UpdateProjectAsync(Project project);
        Task<Project> InactivateProjectAsync(int projectId);
        Task<Project> ActivateProjectAsync(int projectId);
        Task<List<EmployeeProject>> GetByProjectIdAsync(int projectId);
        Task SaveAssignmentsAsync(List<EmployeeProject> employeeProjects);
        Task<Project?> GetProjectDetailByIDAsync(int projectId);

    }
}
