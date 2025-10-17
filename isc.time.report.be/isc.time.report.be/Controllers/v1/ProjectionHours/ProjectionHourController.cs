using isc.time.report.be.application.Interfaces.Service.ProjectionHours;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.ProjectionHours
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectionHourController : ControllerBase
    {
        private readonly IProjectionHourService _service;

        public ProjectionHourController(IProjectionHourService service)
        {
            _service = service;
        }


        [HttpGet("{groupProjection:guid}/get-all")]
        public async Task<ActionResult<List<ProjectionWithoutProjectResponse>>> GetProjectionOfGroup(Guid groupProjection)
        {
            var result = await _service.GetAllProjectionByProjectId(groupProjection);
            return Ok(result);
        }

        // POST: api/projectionwithoutproject/create/{groupProjection}
        [HttpPost("create")]
        public async Task<ActionResult<CreateProjectionWithoutProjectResponse>> CreateProjectionWithoutProject(
            [FromBody] CreateProjectionWithoutProjectRequest request)
        {
            
            var result = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(CreateProjectionWithoutProject), new { groupProjection = result.GroupProjection }, result);
        }

        // PUT: api/projectionwithoutproject/{groupProjection}/update/{resourceTypeId}
        [HttpPut("{groupProjection}/update/{resourceTypeId}")]
        public async Task<ActionResult<UpdateProjectionWithoutProjectRequest>> UpdateProjection(
            Guid groupProjection,
            int resourceTypeId,
            [FromBody] UpdateProjectionWithoutProjectRequest request)
        {
            var result = await _service.UpdateAsync(request, groupProjection, resourceTypeId);
            return Ok(result);
        }


        // PUT: api/projectionwithoutproject/{groupProjection}/activate-inactivate/{resourceTypeId}
        [HttpPut("{groupProjection:guid}/activate-inactivate/{resourceTypeId:int}")]
        public async Task<IActionResult> ActivateInactivateResource(
            Guid groupProjection,
            int resourceTypeId,
            [FromQuery] bool active)
        {
            await _service.ActivateInactiveResourceAsync(groupProjection, resourceTypeId, active);
            return NoContent();
        }

        // GET: api/projectionwithoutproject/{groupProjection}/export-excel
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
