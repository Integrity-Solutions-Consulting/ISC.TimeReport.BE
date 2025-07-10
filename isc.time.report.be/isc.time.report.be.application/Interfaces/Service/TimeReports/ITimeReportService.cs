using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.TimeReports
{
    public interface ITimeReportService
    {
        Task<byte[]> GenerateExcelReportAsync(int employeeId, int year, int month);
    }
}
