using isc.time.report.be.application.Interfaces.Service.TimeReports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.TimeReports
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeReportController : ControllerBase
    {
        private readonly ITimeReportService _timeReportService;

        public TimeReportController(ITimeReportService timeReportService)
        {
            _timeReportService = timeReportService;
        }

        [Authorize (Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo,Colaborador")]
        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] int employeeId, [FromQuery] int clientId, [FromQuery] int year, [FromQuery] int month, [FromQuery] bool fullMonth)
        {
            var fileBytes = await _timeReportService.GenerateExcelReportAsync(employeeId, clientId, year, month, fullMonth);

            var fileName = $"TimeReport_{employeeId}_{year}_{month}.xlsm";
            const string contentType = "application/vnd.ms-excel.sheet.macroEnabled.12";

            return File(fileBytes, contentType, fileName);
        }

        [Authorize (Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo,Colaborador")]
        [HttpGet("recursos-pendientes")] public async Task<IActionResult> GetRecursosPendientes(int? month = null, int? year = null, bool mesCompleto = false)
        {
            var result = await _timeReportService.GetRecursosTimeReportPendienteAsync(month, year, mesCompleto);
            return Ok(result);
        }
    }


}
