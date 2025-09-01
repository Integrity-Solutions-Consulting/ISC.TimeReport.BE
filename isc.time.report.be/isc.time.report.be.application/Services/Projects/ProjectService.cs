using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.application.Interfaces.Repository.Projects;
using isc.time.report.be.application.Interfaces.Service.Projects;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Projects;
using isc.time.report.be.domain.Models.Response.Auth;
using isc.time.report.be.domain.Models.Response.Employees;
using isc.time.report.be.domain.Models.Response.Projects;
using isc.time.report.be.domain.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;
        private readonly IMapper _mapper;
        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            this.projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetAllProjectsResponse>> GetAllProjectsPaginated(PaginationParams paginationParams, string? search)
        {
            var result = await projectRepository.GetAllProjectsPaginatedAsync(paginationParams, search);

            var responseItems = _mapper.Map<List<GetAllProjectsResponse>>(result.Items);

            //foreach (var project in result) 

            return new PagedResult<GetAllProjectsResponse>
            {
                Items = responseItems,
                TotalItems = result.TotalItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<PagedResult<GetAllProjectsResponse>> GetAllProjectsByEmployeeIDPaginated(
            PaginationParams paginationParams,
            string? search,
            int employeeId)
        {
            // Obtiene solo los proyectos asignados al empleado
            var result = await projectRepository.GetAssignedProjectsForEmployeeAsync(paginationParams, search, employeeId);

            var responseItems = _mapper.Map<List<GetAllProjectsResponse>>(result.Items);

            return new PagedResult<GetAllProjectsResponse>
            {
                Items = responseItems,
                TotalItems = result.TotalItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<GetProjectByIDResponse> GetProjectByID(int projectID)
        {
            var project = await projectRepository.GetProjectByIDAsync(projectID);

            if (project == null)
            {
                return null;
            }

            var responseProject = _mapper.Map<GetProjectByIDResponse>(project);

            return responseProject;
        }

        public async Task<CreateProjectResponse> CreateProject(CreateProjectRequest projectRequest)
        {


            var projectNew = await projectRepository.CreateProject( _mapper.Map<Project>(projectRequest));               

            if (projectNew.StartDate > projectNew.EndDate)
            {
                throw new ClientFaultException("No puede ingresar una fecha de Fin anterior a la fecha de Inicio.", 401);
            }

            return _mapper.Map<CreateProjectResponse>(projectNew);
        }
        
        public async Task<UpdateProjectResponse> UpdateProject(int projectId, UpdateProjectRequest projectParaUpdate)
        {
            var projectGet = await projectRepository.GetProjectByIDAsync(projectId);

            if (projectGet == null)
            {
                throw new ClientFaultException("No existe el proyecto", 401);
            }

            projectGet.ClientID = projectParaUpdate.ClientID;
            projectGet.ProjectStatusID = projectParaUpdate.ProjectStatusID;
            projectGet.ProjectTypeID = projectParaUpdate.ProjectTypeID;
            projectGet.Code = projectParaUpdate.Code;
            projectGet.Name = projectParaUpdate.Name;
            projectGet.Description = projectParaUpdate.Description;
            projectGet.StartDate = projectParaUpdate.StartDate;
            projectGet.EndDate = projectParaUpdate.EndDate;
            projectGet.ActualStartDate = projectParaUpdate.ActualStartDate;
            projectGet.ActualEndDate = projectParaUpdate.ActualEndDate;
            projectGet.Budget = projectParaUpdate.Budget;
            projectGet.Hours = projectParaUpdate.Hours;

            if (projectGet.StartDate > projectGet.EndDate)
            {
                throw new ClientFaultException("No puede ingresar una fecha de Fin anterior a la fecha de Anterior.", 401);
            }

            var projectUpdated = await projectRepository.UpdateProjectAsync(projectGet);

            return _mapper.Map<UpdateProjectResponse>(projectUpdated);
        }

        public async Task<ActiveInactiveProjectResponse> InactiveProject(int projectId)
        {

            var projectInactive = await projectRepository.InactivateProjectAsync(projectId);

            return _mapper.Map<ActiveInactiveProjectResponse>(projectInactive);
        }

        public async Task<ActiveInactiveProjectResponse> ActiveProject(int ProjectId)
        {

            var projectActive = await projectRepository.ActivateProjectAsync(ProjectId);

            return _mapper.Map<ActiveInactiveProjectResponse>( projectActive);
        }

        public async Task AssignEmployeesToProject(AssignEmployeesToProjectRequest request)
        {
            foreach (var dto in request.EmployeeProjectMiddle)
            {
                bool tieneEmpleado = dto.EmployeeId.HasValue;
                bool tieneProveedor = dto.SupplierID.HasValue;

                if (tieneEmpleado == tieneProveedor)
                {
                    throw new ArgumentException(
                        $"Cada asignación debe tener solo EmployeeId o solo SupplierID. " +
                        $"DTO con EmployeeId={dto.EmployeeId} SupplierID={dto.SupplierID} no válido."
                    );
                }
            }

            var existing = await projectRepository.GetByProjectEmployeeIDAsync(request.ProjectID);
            var now = DateTime.UtcNow;
            var finalList = new List<EmployeeProject>();

            foreach (var dto in request.EmployeeProjectMiddle)
            {
                var match = existing.FirstOrDefault(ep =>
                    ep.EmployeeID == dto.EmployeeId &&
                    ep.SupplierID == dto.SupplierID
                );

                if (match == null)
                {
                    finalList.Add(new EmployeeProject
                    {
                        ProjectID = request.ProjectID,
                        EmployeeID = dto.EmployeeId,
                        SupplierID = dto.SupplierID,
                        AssignedRole = dto.AssignedRole,
                        CostPerHour = dto.CostPerHour,
                        AllocatedHours = dto.AllocatedHours,
                        Status = true,
                        AssignmentDate = now,
                        CreationDate = now,
                        CreationUser = "SYSTEM"
                    });
                }
                else
                {
                    if (!match.Status)
                    {
                        match.Status = true;
                        match.ModificationDate = now;
                        match.ModificationUser = "SYSTEM";
                    }

                    match.AssignedRole = dto.AssignedRole;
                    match.CostPerHour = dto.CostPerHour;
                    match.AllocatedHours = dto.AllocatedHours;

                    finalList.Add(match);
                }
            }

            foreach (var ep in existing)
            {
                bool sigueEnRequest = request.EmployeeProjectMiddle.Any(dto =>
                    dto.EmployeeId == ep.EmployeeID &&
                    dto.SupplierID == ep.SupplierID
                );

                if (!sigueEnRequest && ep.Status)
                {
                    ep.Status = false;
                    ep.ModificationDate = now;
                    ep.ModificationUser = "SYSTEM";
                    finalList.Add(ep);
                }
            }

            await projectRepository.SaveAssignmentsAsync(finalList);
        }

        public async Task<GetProjectDetailByIDResponse?> GetProjectDetailByID(int projectID)
        {
            var project = await projectRepository.GetProjectDetailByIDAsync(projectID);

            if (project == null)
                return null;

            var response = new GetProjectDetailByIDResponse
            {
                Id = project.Id,
                ClientID = project.ClientID,
                ProjectStatusID = project.ProjectStatusID,
                Code = project.Code,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ActualStartDate = project.ActualStartDate,
                ActualEndDate = project.ActualEndDate,
                Budget = project.Budget,

                EmployeeProjects = project.EmployeeProject.Select(ep => new GetEmployeeProjectResponse
                {
                    Id = ep.Id,
                    EmployeeID = ep.EmployeeID,
                    SupplierID = ep.SupplierID,
                    AssignedRole = ep.AssignedRole,
                    CostPerHour = ep.CostPerHour,
                    AllocatedHours = ep.AllocatedHours,
                    ProjectID = ep.ProjectID,
                    Status = ep.Status
                }).ToList(),

                EmployeesPersonInfo = project.EmployeeProject
                    .Where(ep => ep.Employee != null)
                    .Select(ep => ep.Employee)
                    .Distinct()
                    .Select(e => new GetEmployeesPersonInfoResponse
                    {
                        Id = e.Id,
                        PersonID = e.PersonID,
                        EmployeeCode = e.EmployeeCode,
                        IdentificationNumber = e.Person.IdentificationNumber,
                        FirstName = e.Person.FirstName,
                        LastName = e.Person.LastName,
                        Status = e.Status
                    }).ToList()
            };

            return response;
        }

        public async Task<List<GetProjectsByEmployeeIDResponse>> GetProjectsByEmployeeIdAsync(int employeeId)
        {
            var projects = await projectRepository.GetProjectsByEmployeeIdAsync(employeeId);
            return _mapper.Map<List<GetProjectsByEmployeeIDResponse>>(projects);
        }
    }
}
