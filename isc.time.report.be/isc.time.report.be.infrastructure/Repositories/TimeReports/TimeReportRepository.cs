using isc.time.report.be.application.Interfaces.Repository.TimeReports;
using isc.time.report.be.domain.Entity.DailyActivities;
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

        public async Task<List<DailyActivity>> GetActivitiesByEmployeeAndProjectsAsync(int employeeId, List<int> projectIds)
        {
            return await _dbContext.DailyActivities
                .Include(a => a.ActivityType)
                .Include(a => a.Project)
                .Where(a => a.EmployeeID == employeeId && projectIds.Contains(a.ProjectID ?? 0))
                .ToListAsync();
        }

    }
}
