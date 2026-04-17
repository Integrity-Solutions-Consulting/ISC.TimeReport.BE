using isc.time.report.be.application.Interfaces.Service.Homologaciones;
using isc.time.report.be.domain.Models.Request.Homologacion;
using isc.time.report.be.domain.Models.Response.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Homologacion
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class HomologacionController : ControllerBase
    {
        private readonly IHomologacionService _service;

        public HomologacionController(IHomologacionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(new SuccessResponse<object>(200, "Homologaciones obtenidas correctamente", result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHomologacionRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(new SuccessResponse<object>(201, "Homologación creada exitosamente", result));
        }
    }
}
