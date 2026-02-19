using isc.time.report.be.application.Interfaces.Service.ProjectionHours;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.ProjectionHours
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectionHourController : ControllerBase
    {
        private readonly IProjectionHourService _service;

        public ProjectionHourController(IProjectionHourService service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<List<ProjectionWithoutProjectResponse>>>> GetAllProjectionsByGuid()
        {
            var result = await _service.GetAllProjectionWithoutProjectAsync();
            return Ok(result);
        }

        [HttpGet("{groupProjection}/get-by-guid")]
        public async Task<ActionResult<List<ProjectionWithoutProjectResponse>>> GetProjectionOfGroup(Guid groupProjection)
        {
            var result = await _service.GetAllProjectionByProjectId(groupProjection);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateProjectionWithoutProjectResponse>> CreateProjectionWithoutProject(
            [FromBody] CreateProjectionWithoutProjectRequest request)
        {

            var result = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(CreateProjectionWithoutProject), new { groupProjection = result.GroupProjection }, result);
        }

        [HttpPut("{groupProjection}/update/{resourceTypeId}")]
        public async Task<ActionResult<UpdateProjectionWithoutProjectRequest>> UpdateProjection(
            Guid groupProjection,
            int resourceTypeId,
            [FromBody] UpdateProjectionWithoutProjectRequest request)
        {
            var result = await _service.UpdateAsync(request, groupProjection, resourceTypeId);
            return Ok(result);
        }


        [HttpPut("{groupProjection:guid}/activate-inactivate/{resourceTypeId:int}")]
        public async Task<IActionResult> ActivateInactivateResource(
            Guid groupProjection,
            int resourceTypeId,
            [FromQuery] bool active)
        {
            await _service.ActivateInactiveResourceAsync(groupProjection, resourceTypeId, active);
            return NoContent();
        }

        [HttpGet("{groupProjection:guid}/export-excel")]
        public async Task<IActionResult> ExportProjectionExcel(Guid groupProjection)
        {
            try
            {
                byte[] fileBytes = await _service.ExportProjectionWithoutProjectToExcelAsync(groupProjection);
                string fileName = $"Proyeccion_Grupo_{groupProjection}.xlsx";

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
