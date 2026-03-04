using isc.time.report.be.domain.Models.Dto.TimeReports;
using isc.time.report.be.domain.Models.Response.Dashboards;

namespace isc.time.report.be.application.Interfaces.Service.TimeReports
{
    public interface ITimeReportService
    {
        Task<byte[]> GenerateExcelReportAsync(int employeeId, int clientId, int year, int month, bool fullMonth);
        Task<TimeReportDataFillDto> GetTimeReportDataFillAsync(int employeeId, int clientId, int year, int month, bool fullMonth);
        Task<List<DashboardRecursosPendientesDto>> GetRecursosTimeReportPendienteAsync(int? month = null, int? year = null, bool mesCompleto = false);
        Task<List<DashboardRecursosPendientesDto>> GetRecursosTimeReportPendienteFiltradoAsync(int? month = null, int? year = null, bool mesCompleto = false, byte bancoGuayaquil = 0);
        Task<byte[]> GenerateExcelModelAsync();
    }
}
