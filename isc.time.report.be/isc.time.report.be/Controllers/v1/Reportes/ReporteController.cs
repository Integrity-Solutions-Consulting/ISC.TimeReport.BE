using isc.time.report.be.application.Interfaces.Service.Reportes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Reportes
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : Controller
    {

        private readonly IReporteService _service;

        public ReporteController(IReporteService service)
        {
            _service = service;
        }



        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("recursos-cliente")]
        public async Task<IActionResult> ObtenerReporte()
        {
            var result = await _service.ObtenerReporteClienteAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("recursos-proyecto")]
        public async Task<IActionResult> ObtenerReporteAsync()
        {
            var result = await _service.ObtenerReporteProyectoAsync();
            return Ok(result);
        }




    }
}
