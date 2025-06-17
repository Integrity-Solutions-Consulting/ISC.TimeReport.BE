using isc.time.report.be.application.Interfaces.Service.Persons;
using isc.time.report.be.application.Interfaces.Service.Projects;
using isc.time.report.be.application.Services.Persons;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Projects;
using isc.time.report.be.domain.Models.Response.Persons;
using isc.time.report.be.domain.Models.Response.Projects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Projects
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("GetAllProjects")]
        public async Task<ActionResult<SuccessResponse<PagedResult<GetAllProjectsResponse>>>> GetAllProjects([FromQuery] PaginationParams paginationParams)
        {
            var projects = await _projectService.GetAllProjectsPaginated(paginationParams);
            return Ok((projects));
        }

        [HttpGet("GetProjectByID/{id}")]
        public async Task<ActionResult<SuccessResponse<GetProjectByIDResponse>>> GetProjectBYID(int id)
        {
            var project = await _projectService.GetProjectByID(id);
            return Ok(project);
        }

        [HttpPost("CreateProject")]
        public async Task<ActionResult<SuccessResponse<CreateProjectResponse>>> CreateProject(CreateProjectRequest request)
        {
            var project = await _projectService.CreateProject(request);

            return Ok(project);
        }

        [HttpPut("UpdateProjectByID/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdateProjectResponse>>> UpdateProjectById(
            int id,
            [FromBody] UpdateProjectRequest request)
        {
            var projectUpdate = await _projectService.UpdateProject(id, request);

            return Ok(projectUpdate);
        }

        [HttpDelete("InactiveProjectByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactiveProjectResponse>>> InactiveProjectById(int id)
        {
            var inactiveProject = await _projectService.InactiveProject(id);

            return Ok(inactiveProject);
        }

        [HttpDelete("ActiveProjectByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactiveProjectResponse>>> ActiveProjectById(int id)
        {
            var ActiveProject = await _projectService.ActiveProject(id);

            return Ok(ActiveProject);
        }
    }
}
