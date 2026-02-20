using isc.time.report.be.application.Interfaces.Repository.Report;
using isc.time.report.be.application.Interfaces.Service.Report;
using static isc.time.report.be.domain.Models.Response.Report.ReportResponse;

namespace isc.time.report.be.application.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ProjectResourcesReportDto>> GetProjectReportAsync()
        {
            var data = await _repo.GetResourcesReportAsync();
            return data;

        }

        public async Task<List<ClientHourlyResourceAmountDto>> GetClientReportAsync()
        {
            var data = await _repo.GetResourceAndHoursCountByClientAsync();
            return data;
        }

    }
}
