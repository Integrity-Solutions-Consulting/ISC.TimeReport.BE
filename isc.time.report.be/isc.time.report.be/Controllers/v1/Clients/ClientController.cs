//using isc.time.report.be.domain.Models.Dto;
//using isc.time.report.be.domain.Models.Request.Projects;
//using isc.time.report.be.domain.Models.Response.Clients;
//using isc.time.report.be.domain.Models.Response.Projects;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace isc.time.report.be.api.Controllers.v1.Clients
//{

//    [ApiExplorerSettings(GroupName = "v1")]
//    [ApiController]
//    [Route("api/[Contorller]")]
//    public class ClientController : ControllerBase
//    {
//        private readonly IClientService _clientService;
//        public ClientController(IClientService clientService)
//        {
//            this._clientService = clientService;
//        }

//        //[HttpGet("GetAllClients")]
//        //public async Task<ActionResult<SuccessResponse<List<GetClientDetail>>>> GetAllClients()
//        //{
//        //    var Clients = await _clientService.GetAllProjects();
//        //    return Ok(projects);
//        //}











//        //[HttpGet("GetProjectByID/{id}")]
//        //public async Task<ActionResult<SuccessResponse<GetProjectByIDResponse>>> GetProjectBYID(int id)
//        //{
//        //    var project = await _projectService.GetProjectByID(id);
//        //    return Ok(project);
//        //}

//        //[HttpPost("CreateProject")]
//        //public async Task<ActionResult<SuccessResponse<CreateProjectResponse>>> CreateProject(CreateProjectRequest request)
//        //{
//        //    var project = await _projectService.CreateProject(request);

//        //    return Ok(project);
//        //}

//        //[HttpPut("UpdateProjectByID/{id}")]
//        //public async Task<ActionResult<SuccessResponse<UpdateProjectResponse>>> UpdateProjectById(
//        //    int id,
//        //    [FromBody] UpdateProjectRequest request)
//        //{
//        //    var projectUpdate = await _projectService.UpdateProject(id, request);

//        //    return Ok(projectUpdate);
//        //}

//        //[HttpDelete("InactiveProjectByID/{id}")]
//        //public async Task<ActionResult<SuccessResponse<ActiveInactiveProjectResponse>>> InactiveProjectById(int id)
//        //{
//        //    var inactiveProject = await _projectService.InactiveProject(id);

//        //    return Ok(inactiveProject);
//        //}

//        //[HttpDelete("ActiveProjectByID/{id}")]
//        //public async Task<ActionResult<SuccessResponse<ActiveInactiveProjectResponse>>> ActiveProjectById(int id)
//        //{
//        //    var ActiveProject = await _projectService.ActiveProject(id);

//        //    return Ok(ActiveProject);
//        //}
//    }
//}
