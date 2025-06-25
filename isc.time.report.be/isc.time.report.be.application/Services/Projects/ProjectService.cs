using AutoMapper;
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

        public async Task<PagedResult<GetAllProjectsResponse>> GetAllProjectsPaginated(PaginationParams paginationParams)
        {
            var result = await projectRepository.GetAllProjectsPaginatedAsync(paginationParams);

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
                throw new ClientFaultException("No puede ingresar una fecha de Inicio anterior a la fecha de fin.", 401);
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
            projectGet.Code = projectParaUpdate.Code;
            projectGet.Name = projectParaUpdate.Name;
            projectGet.Description = projectParaUpdate.Description;
            projectGet.StartDate = projectParaUpdate.StartDate;
            projectGet.EndDate = projectParaUpdate.EndDate;
            projectGet.ActualStartDate = projectParaUpdate.ActualStartDate;
            projectGet.ActualEndDate = projectParaUpdate.ActualEndDate;
            projectGet.Budget = projectParaUpdate.Budget;

            if (projectGet.StartDate < projectGet.EndDate)
            {
                throw new ClientFaultException("No puede ingresar una fecha de Inicio anterior a la fecha de fin.", 401);
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
            var existing = await projectRepository.GetByProjectIdAsync(request.ProjectID);

            var finalList = new List<EmployeeProject>();

            var now = DateTime.Now;

            foreach (var empId in request.EmployeeIDs)
            {
                var match = existing.FirstOrDefault(e => e.EmployeeID == empId);

                if (match == null)
                {
                    finalList.Add(new EmployeeProject
                    {
                        EmployeeID = empId,
                        ProjectID = request.ProjectID,
                        Status = true,
                        AssignmentDate = now,
                        CreationDate = now,
                        CreationUser = "SYSTEM"
                    });
                }
                else if (!match.Status)
                {
                    match.Status = true;
                    match.ModificationDate = now;
                    match.ModificationUser = "SYSTEM";
                    finalList.Add(match);
                }
                else
                {
                    finalList.Add(match);
                }
            }

            foreach (var ep in existing)
            {
                if (!request.EmployeeIDs.Contains(ep.EmployeeID))
                {
                    if (ep.Status)
                    {
                        ep.Status = false;
                        ep.ModificationDate = now;
                        ep.ModificationUser = "SYSTEM";
                    }
                    finalList.Add(ep);
                }
            }

            await projectRepository.SaveAssignmentsAsync(finalList);
        }
    }
}
