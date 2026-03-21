using static isc.time.report.be.domain.Models.Response.Report.ReportResponse;

namespace isc.time.report.be.application.Interfaces.Service.Report
{
    public interface IReportService
    {
        Task<List<ProjectResourcesReportDto>> GetProjectReportAsync();
        Task<List<ClientHourlyResourceAmountDto>> GetClientReportAsync();

    }
}
