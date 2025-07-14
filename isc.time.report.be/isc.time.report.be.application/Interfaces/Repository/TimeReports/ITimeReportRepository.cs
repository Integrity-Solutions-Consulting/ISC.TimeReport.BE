using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Holidays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.TimeReports
{
    public interface ITimeReportRepository
    {

        Task<List<int>> GetProjectIdsForEmployeeByClientAsync(int employeeId, int clientId);

        Task<List<DailyActivity>> GetActivitiesByEmployeeAndProjectsAsync(int employeeId, List<int> projectIds);
        Task<List<Holiday>> GetActiveHolidaysByMonthAndYearAsync(int month, int year);
    }
}
