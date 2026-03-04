using isc.time.report.be.application.Interfaces.Repository.Projects;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DBContext _dbContext;

        public ProjectRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            ;
        }

        public async Task<PagedResult<Project>> GetAllProjectsPaginatedAsync(PaginationParams paginationParams, string? search)
        {
            var query = _dbContext.Projects
                .Include(p => p.Leader)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();

                query = query.Where(p =>
                    p.Name.ToLower().Contains(normalizedSearch) ||
                    p.Code.ToLower().Contains(normalizedSearch));
            }

            query = query.OrderBy(p => p.Status ? 0 : 1)
                         .ThenBy(p => p.Name);

            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<PagedResult<Project>> GetAssignedProjectsForEmployeeAsync(
            PaginationParams paginationParams, string? search, int employeeId)
        {
            DateTime now = DateTime.Now;
            DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);   // 1er día del mes actual
            DateTime startOfNextMonth = startOfMonth.AddMonths(
                1);          // 1er día del mes siguiente

            IQueryable<Project> query = _dbContext.Projects
                .Where(p => p.Status == true
                            && p.EmployeeProject.Any(ep => ep.EmployeeID == employeeId && ep.Status == true)
                            && p.EndDate.HasValue
                            && p.EndDate.Value >= startOfMonth);


            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(normalizedSearch) ||
                    p.Code.ToLower().Contains(normalizedSearch));
            }

            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<PagedResult<Project>> GetAssignedProjectsForEmployeeActiveAsync(
    PaginationParams paginationParams, string? search, int employeeId)
        {
            DateTime now = DateTime.Now;
            DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);   // 1er día del mes actual
            DateTime startOfNextMonth = startOfMonth.AddMonths(
                1);          // 1er día del mes siguiente

            IQueryable<Project> query = _dbContext.Projects
                .Where(p => p.Status == true
                            && p.EmployeeProject.Any(ep => ep.EmployeeID == employeeId && ep.Status == true)
                            && p.EndDate.HasValue
                            && p.EndDate.Value >= startOfMonth);


            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(normalizedSearch) ||
                    p.Code.ToLower().Contains(normalizedSearch));
            }

            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<Project> GetProjectByIDAsync(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ClientFaultException("El ID no puede ser menor a 0");
            }
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null)
            {
                throw new ClientFaultException($"No se encontró el proyecto con ID {projectId}.");
            }
            return project;
        }
        public async Task<Project> CreateProject(Project project)
        {
            var existingProject = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Code == project.Code);

            if (existingProject != null)
            {
                throw new ClientFaultException($"Ya existe un proyecto con ese código '{project.Code}'.");
            }

            project.CreationDate = DateTime.Now;
            project.ModificationDate = null;
            project.Status = true;

            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);

            return project;
        }
        public async Task<Project> UpdateProjectAsync(Project project)
        {
            // Obtiene la entidad existente
            Project trackedEntity = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == project.Id);

            if (trackedEntity == null)
                return null; // o lanzar excepción según tu manejo de errores

            // Actualiza todos los valores
            _dbContext.Entry(trackedEntity).CurrentValues.SetValues(project);

            // Marca la entidad como modificada explícitamente
            _dbContext.Entry(trackedEntity).State = EntityState.Modified;

            // Actualiza campos de auditoría
            trackedEntity.ModificationDate = DateTime.Now;
            trackedEntity.ModificationUser = "SYSTEM";

            // Guarda cambios en la base
            await _dbContext.SaveChangesAsync();

            return trackedEntity;
        }




        public async Task<Project> InactivateProjectAsync(int projectId)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new ClientFaultException($"El proyecto con ID {projectId} no existe.");

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
                throw new ClientFaultException($"El proyecto con ID {projectId} no existe.");

            project.Status = true;
            project.ModificationDate = DateTime.Now;

            _dbContext.Entry(project).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<List<EmployeeProject>> GetByProjectEmployeeIDAsync(int projectId)
        {
            var project = await _dbContext.EmployeeProjects
                .Where(ep => ep.ProjectID == projectId)
                .ToListAsync();
            //if (!project.Any())
            //{
            //    throw new ServerFaultException("No existen projectos con esa ID");
            //}
            return project;
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

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;

                // Lanza una excepción personalizada que tu middleware puede manejar
                throw new ServerFaultException(
                    message: $"Error al guardar los cambios: {innerMessage}",
                    code: 500,
                    innerException: dbEx
                );
            }
            catch (Exception ex)
            {
                throw new ServerFaultException(
                    message: "Ocurrió un error inesperado al guardar las asignaciones.",
                    code: 500,
                    innerException: ex
                );
            }
        }


        public async Task<Project> GetProjectDetailByIDAsync(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ClientFaultException("El ID del proyecto no puede ser menor o igual a 0.");
            }

            var project = await _dbContext.Projects
                .Include(p => p.EmployeeProject)
                    .ThenInclude(ep => ep.Employee)
                        .ThenInclude(e => e.Person)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new ClientFaultException($"No se encontró el proyecto con ID {projectId}.");
            }

            return project;
        }


        public async Task<List<Project>> GetProjectsByEmployeeIdAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ClientFaultException("El ID del empleado no puede ser menor o igual a 0.");
            }

            var project = await _dbContext.EmployeeProjects
                .Where(ep => ep.EmployeeID == employeeId)
                .Select(ep => ep.Project)
                .Distinct()
                .ToListAsync();
            if (!project.Any())
            {
                throw new ServerFaultException($"No se encontraron proyectos asignados al empleado con ID {employeeId}.");
            }
            return project;
        }

        public async Task<List<int>> GetProjectToEmployeeAsync(int employeeId)
        {
            var projects = await _dbContext.EmployeeProjects
                .Where(ep => ep.EmployeeID == employeeId)
                .Select(ep => ep.ProjectID)
                .ToListAsync();

            return projects;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _dbContext.Projects
                .Include(p => p.Client)
                    .ThenInclude(c => c.Person)
                .Include(p => p.Leader)
                .Include(p => p.ProjectStatus)
                .Include(p => p.ProjectType)
                .OrderBy(p => p.Status ? 0 : 1)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }

    }
}
