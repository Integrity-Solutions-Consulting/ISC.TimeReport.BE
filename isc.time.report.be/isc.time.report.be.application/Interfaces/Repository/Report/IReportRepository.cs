
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static isc.time.report.be.domain.Models.Response.Report.ReportResponse;

namespace isc.time.report.be.application.Interfaces.Repository.Report
{
    public interface IReportRepository
    {

        Task<List<ProjectResourcesReportDto>> GetResourcesReportAsync();
        Task<List<ClientHourlyResourceAmountDto>> GetResourceAndHoursCountByClientAsync();


    }
}
