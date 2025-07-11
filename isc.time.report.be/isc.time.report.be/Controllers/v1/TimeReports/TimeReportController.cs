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

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] int employeeId, [FromQuery] int clientId, [FromQuery] int year, [FromQuery] int month)
        {
            var fileBytes = await _timeReportService.GenerateExcelReportAsync(employeeId, clientId, year, month);

            var fileName = $"TimeReport_{employeeId}_{year}_{month}.xlsm";
            const string contentType = "application/vnd.ms-excel.sheet.macroEnabled.12";

            return File(fileBytes, contentType, fileName);
        }
    }


}
