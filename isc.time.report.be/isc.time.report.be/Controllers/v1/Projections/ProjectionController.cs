using isc.time.report.be.application.Interfaces.Service.Projections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Projections
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProjectionController : ControllerBase
    {
        private readonly IProjectionHourProjectService _service;
        public ProjectionController (IProjectionHourProjectService service)
        {
           _service = service;
        }

        //[Authorize(Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo,Colaborador")]
        [HttpGet("get-all-projection-by-projectId/{projectId:int}")]
        public async Task<IActionResult> GetProyeccionesPorProyecto(int projectId)
        {
            var result = await _service.GetAllProjectionByProjectId(projectId);
            return Ok(result);
        }
    }
}
