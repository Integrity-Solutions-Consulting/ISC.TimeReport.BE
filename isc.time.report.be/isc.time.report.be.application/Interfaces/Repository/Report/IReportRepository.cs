using static isc.time.report.be.domain.Models.Response.Report.ReportResponse;

namespace isc.time.report.be.application.Interfaces.Repository.Report
{
    public interface IReportRepository
    {

        Task<List<ProjectResourcesReportDto>> GetResourcesReportAsync();
        Task<List<ClientHourlyResourceAmountDto>> GetResourceAndHoursCountByClientAsync();


    }
}
