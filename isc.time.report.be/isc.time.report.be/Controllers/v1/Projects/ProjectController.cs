using isc.time.report.be.application.Interfaces.Service.Persons;
using isc.time.report.be.application.Interfaces.Service.Projects;
using isc.time.report.be.application.Services.Persons;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Projects;
using isc.time.report.be.domain.Models.Response.Persons;
using isc.time.report.be.domain.Models.Response.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        private int GetEmployeeIdFromToken()
            => int.Parse(User.Claims.First(c => c.Type == "EmployeeID").Value);

        private int GetUserIdFromToken()
            => int.Parse(User.Claims.First(c => c.Type == "UserID").Value);

        private List<string> GetRolesFromToken()
            => User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();


        [Authorize(Roles = "Administrador,Gerente,Lider,Colaborador")]
        [HttpGet("GetAllProjects")]
        public async Task<ActionResult<SuccessResponse<PagedResult<GetAllProjectsResponse>>>> GetAllProjects([FromQuery] PaginationParams paginationParams, [FromQuery] string? search)
        {
            var projects = await _projectService.GetAllProjectsPaginated(paginationParams, search);
            return Ok(projects);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Colaborador")]
        [HttpGet("GetAllProjectsWhereEmployee")]
        public async Task<ActionResult<SuccessResponse<PagedResult<GetAllProjectsResponse>>>> GetAllProjectsByEmployeeID(
        [FromQuery] PaginationParams paginationParams,
        [FromQuery] string? search)
        {
            int employeeId = GetEmployeeIdFromToken();

            var projects = await _projectService.GetAllProjectsByEmployeeIDPaginated(paginationParams, search, employeeId);
            return Ok(projects);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("GetProjectByID/{id}")]
        public async Task<ActionResult<SuccessResponse<GetProjectByIDResponse>>> GetProjectBYID(int id)
        {
            var project = await _projectService.GetProjectByID(id);
            return Ok(project);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPost("CreateProject")]
        public async Task<ActionResult<SuccessResponse<CreateProjectResponse>>> CreateProject(CreateProjectRequest request)
        {
            var project = await _projectService.CreateProject(request);

            return Ok(project);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPut("UpdateProjectByID/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdateProjectResponse>>> UpdateProjectById(
            int id,
            [FromBody] UpdateProjectRequest request)
        {
            var projectUpdate = await _projectService.UpdateProject(id, request);

            return Ok(projectUpdate);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpDelete("InactiveProjectByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactiveProjectResponse>>> InactiveProjectById(int id)
        {
            var inactiveProject = await _projectService.InactiveProject(id);

            return Ok(inactiveProject);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpDelete("ActiveProjectByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactiveProjectResponse>>> ActiveProjectById(int id)
        {
            var ActiveProject = await _projectService.ActiveProject(id);

            return Ok(ActiveProject);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPost("AssignEmployeesToProject")]
        public async Task<IActionResult> AssignEmployeesToProject([FromBody] AssignEmployeesToProjectRequest request)
        {
            await _projectService.AssignEmployeesToProject(request);
            return Ok(new { message = "Asignaciones actualizadas correctamente." });
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("GetProjectDetailByID/{id}")]
        public async Task<ActionResult<SuccessResponse<GetProjectDetailByIDResponse>>> GetProjectDetailByID(int id)
        {
            var result = await _projectService.GetProjectDetailByID(id);

            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Colaborador")]
        [HttpGet("get-projects-by-employee")]
        public async Task<ActionResult<SuccessResponse<List<GetProjectsByEmployeeIDResponse>>>>GetProjectsByEmployee()
        {
            int employeeId = GetEmployeeIdFromToken();

            var list = await _projectService.GetProjectsByEmployeeIdAsync(employeeId);
            return Ok(list);
        }
    }
}
