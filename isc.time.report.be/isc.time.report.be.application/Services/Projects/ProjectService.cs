using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.application.Interfaces.Repository.Projects;
using isc.time.report.be.application.Interfaces.Service.Projects;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Projects;
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
        public ProjectService(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<List<GetAllProjectsResponse>> GetAllProjects()
        {

            var projects = await projectRepository.GetAllProjectsAsync();

            return projects.Select(p => new GetAllProjectsResponse
            {
                ProjectID = p.Id,
                ClientID = p.ClientID,
                ProjectStatusID = p.ProjectStatusID,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ActualStartDate = p.ActualStartDate,
                ActualEndDate = p.ActualEndDate,
                Budget = p.Budget,
            }).ToList();
        }

        public async Task<GetProjectByIDResponse> GetProjectByID(int projectID)
        {
            var project = await projectRepository.GetProjectByIDAsync(projectID);

            if (project == null)
            {
                return null;
            }

            var responseProject = new GetProjectByIDResponse
            {
                ProjectID = project.Id,
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
            };

            return responseProject;
        }

        public async Task<CreateProjectResponse> CreateProject(CreateProjectRequest projectRequest)
        {


            var projectNew = await projectRepository.CreateProject(new Project
            {
                ClientID = projectRequest.ClientID,
                ProjectStatusID = projectRequest.ProjectStatusID,
                Code = projectRequest.Code,
                Name = projectRequest.Name,
                Description = projectRequest.Description,
                StartDate = projectRequest.StartDate,
                EndDate = projectRequest.EndDate,
                Budget = projectRequest.Budget,
            });

            if (projectNew.StartDate < projectNew.EndDate)
            {
                throw new ClientFaultException("No puede ingresar una fecha de Inicio anterior a la fecha de fin.", 401);
            }

            return new CreateProjectResponse
            {
                ProjectID = projectNew.Id,
                ClientID = projectNew.ClientID,
                ProjectStatusID = projectNew.ProjectStatusID,
                Code = projectNew.Code,
                Name = projectNew.Name,
                Description = projectNew.Description,
                StartDate = projectNew.StartDate,
                EndDate = projectNew.EndDate,
                Budget = projectNew.Budget,
            };
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

            var projectUpdated = await projectRepository.UpdateProjectAsync(projectGet);

            return new UpdateProjectResponse
            {
                ProjectID = projectUpdated.Id,
                ClientID = projectUpdated.ClientID,
                ProjectStatusID= projectUpdated.ProjectStatusID,
                Code = projectUpdated.Code,
                Name = projectUpdated.Name,
                Description = projectUpdated.Description,
                StartDate = projectUpdated.StartDate,
                EndDate = projectUpdated.EndDate,
                ActualStartDate = projectUpdated.ActualStartDate,
                ActualEndDate = projectUpdated.ActualEndDate,
                Budget = projectUpdated.Budget,

            };
        }

        public async Task<ActiveInactiveProjectResponse> InactiveProject(int projectId)
        {

            var projectInactive = await projectRepository.InactivateProjectAsync(projectId);

            return new ActiveInactiveProjectResponse {

                ProjectID = projectInactive.Id,
                ClientID = projectInactive.ClientID,
                ProjectStatusID = projectInactive.ProjectStatusID,
                Code = projectInactive.Code,
                Name = projectInactive.Name,
                Description = projectInactive.Description,
                StartDate = projectInactive.StartDate,
                EndDate = projectInactive.EndDate,
                ActualStartDate = projectInactive.ActualStartDate,
                ActualEndDate= projectInactive.ActualEndDate,
                Budget= projectInactive.Budget,
                Status = projectInactive.Status,

            };

        }

        public async Task<ActiveInactiveProjectResponse> ActiveProject(int ProjectId)
        {

            var projectActive = await projectRepository.ActivateProjectAsync(ProjectId);

            return new ActiveInactiveProjectResponse
            {

                ProjectID = projectActive.Id,
                ClientID = projectActive.ClientID,
                ProjectStatusID = projectActive.ProjectStatusID,
                Code = projectActive.Code,
                Name = projectActive.Name,
                Description = projectActive.Description,
                StartDate = projectActive.StartDate,
                EndDate = projectActive.EndDate,
                ActualStartDate = projectActive.ActualStartDate,
                ActualEndDate = projectActive.ActualEndDate,
                Budget = projectActive.Budget,
                Status = projectActive.Status,
            };

        }
    }
}
