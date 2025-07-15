using isc.time.report.be.application.Interfaces.Repository.TimeReports;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Holidays;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.TimeReports
{
    public class TimeReportRepository : ITimeReportRepository
    {
        private readonly DBContext _dbContext;

        public TimeReportRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<int>> GetProjectIdsForEmployeeByClientAsync(int employeeId, int clientId)
        {
            return await _dbContext.EmployeeProjects
                .Where(ep => ep.EmployeeID == employeeId && ep.Project.ClientID == clientId)
                .Select(ep => ep.ProjectID)
                .ToListAsync();
        }

        public async Task<List<DailyActivity>> GetActivitiesByEmployeeAndProjectsAsync(
            int employeeId,
            List<int> projectIds,
            int year,
            int month,
            bool fullMonth)
        {
            var startDate = new DateOnly(year, month, 1);
            var endDate = fullMonth
                ? startDate.AddMonths(1).AddDays(-1)
                : new DateOnly(year, month, 15);

            return await _dbContext.DailyActivities
                .Include(a => a.ActivityType)
                .Include(a => a.Project)
                .Where(a => a.EmployeeID == employeeId
                    && projectIds.Contains(a.ProjectID ?? 0)
                    && a.Status
                    && a.ActivityDate >= startDate
                    && a.ActivityDate <= endDate)
                .ToListAsync();
        }

        public async Task<List<Holiday>> GetActiveHolidaysByMonthAndYearAsync(int month, int year)
        {
            return await _dbContext.Holidays
                .Where(h => h.Status == true && (
                    (h.IsRecurring && h.HolidayDate.Month == month) ||
                    (!h.IsRecurring && h.HolidayDate.Month == month && h.HolidayDate.Year == year)
                ))
                .ToListAsync();
        }



    }
}
