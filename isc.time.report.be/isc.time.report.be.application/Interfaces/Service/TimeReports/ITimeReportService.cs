using isc.time.report.be.domain.Models.Dto.TimeReports;
using isc.time.report.be.domain.Models.Response.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.TimeReports
{
    public interface ITimeReportService
    {
        Task<byte[]> GenerateExcelReportAsync(int employeeId, int clientId, int year, int month, bool fullMonth);
        Task<TimeReportDataFillDto> GetTimeReportDataFillAsync(int employeeId, int clientId, int year, int month, bool fullMonth);
        Task<List<DashboardRecursosPendientesDto>> GetRecursosTimeReportPendienteAsync(int? month = null, int? year = null);
    }
}
