using isc.time.report.be.application.Interfaces.Service.Projections;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
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

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("{projectId:int}/get-all-projection-by-projectId")]
        public async Task<ActionResult<List<ProjectionHoursProjectResponse>>> GetProjectionOfProject (int projectId)
        {
            var result = await _service.GetAllProjectionByProjectId(projectId);
            return Ok(result);
        }

        [HttpGet("{projectId:int}/get-all-projection-withou-projectid")]
        public async Task<ActionResult<ProjectionWithoutProjectResponse>> GetProjectionById(Guid projectionId)
        {
            var result = await _service.GetProjectionWithoutProjectByIdAsync(
                new GetProjectionWithoutProjectByIdRequest { Id = projectionId });
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPost("create")]
        public async Task<ActionResult<ProjectionHoursProjectRequest>> CreateProjection([FromBody] ProjectionHoursProjectRequest request, [FromRoute] int projectId)
        {
            var result = await _service.CreateAsync(request, projectId);
            return CreatedAtAction(nameof(CreateProjection), new { projectId = result.ProjecId }, result);
        }

        [HttpPost("create-without-project")]
        public async Task<ActionResult<CreateProjectionWithoutProjectResponse>> CreateProjection(
          [FromBody] CreateProjectionWithoutProjectRequest request)
        {
            var result = await _service.CreateProjectionWithoutProjectAsync(request);
            return CreatedAtAction(nameof(CreateProjection), new { id = result.Id }, result);
        }


        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPut("{projectId:int}/update/{resourceTypeId:int}")]
        public async Task<ActionResult<UpdateProjectionHoursProjectRequest>> UpdateProjection(
            int projectId,
            int resourceTypeId,
            [FromBody] UpdateProjectionHoursProjectRequest request)
        {
            var result = await _service.UpdateAsync(request, resourceTypeId, projectId);
            return Ok(result);
        }

        [HttpPut("{projectionId:guid}/update/{resourceTypeId:int}")]
        public async Task<ActionResult<UpdateProjectionWithoutProjectResponse>> UpdateProjectionResource(
            Guid projectionId,
            int resourceTypeId,
            [FromBody] UpdateProjectionWithoutProjectRequest request)
        {
            // Asignamos solo el ProjectionId al request
            request.Id = projectionId;

            var result = await _service.UpdateProjectionWithooutProjectAsync(request, resourceTypeId);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPut("{projectId:int}/activate-inactivate/{resourceTypeId:int}")]
        public async Task<IActionResult> ActivateInactivateResource(
            int projectId,
            int resourceTypeId,
            [FromQuery] bool active)
        {
            await _service.ActivateInactiveResourceAsync(projectId, resourceTypeId, active);
            return NoContent(); 
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("{projectId:int}/export-excel")]
        public async Task<IActionResult> ExportProjectionExcel(int projectId)
        {
            try
            {
                byte[] fileBytes = await _service.ExportProjectionToExcelAsync(projectId);
                string fileName = $"Proyeccion_Proyecto_{projectId}.xlsx";

                return File(fileBytes,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}

