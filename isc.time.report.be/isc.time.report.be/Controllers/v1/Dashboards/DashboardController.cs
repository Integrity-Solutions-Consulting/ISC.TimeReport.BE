using isc.time.report.be.application.Interfaces.Service.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Dashboards
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("resumen-general")]
        public async Task<IActionResult> GetDashboardResumenGeneral()
        {
            var result = await _service.GetDashboardResumenGeneralAsync();
            return Ok(result);
        }

        [HttpGet("horas-por-actividad")]
        public async Task<IActionResult> GetHorasPorActividad([FromQuery] DateOnly? fecha)
        {
            var result = await _service.GetHorasPorActividadPorFechaAsync(fecha);
            return Ok(result);
        }

        [HttpGet("recursos-por-cliente")] public async Task<IActionResult> GetRecursosPorCliente()
        {
            var result = await _service.GetRecursosPorClienteAsync();
            return Ok(result);
        }

        [HttpGet("resumen-proyectos")]
        public async Task<IActionResult> GetResumenProyectos([FromQuery] string? tipoFiltro, [FromQuery] string? valor)
        {
            var result = await _service.GetResumenProyectosAsync(tipoFiltro, valor);
            return Ok(result);
        }
    }
}
