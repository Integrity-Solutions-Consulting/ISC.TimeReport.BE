using isc.time.report.be.domain.Entity.DailyActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.DailyActivities
{
    public interface IDailyActivityRepository
    {
        Task<List<DailyActivity>> GetAllAsync();
        Task<DailyActivity?> GetByIdAsync(int id);
        Task<DailyActivity> CreateAsync(DailyActivity entity);
        Task<DailyActivity> UpdateAsync(DailyActivity entity);
        Task<DailyActivity> InactivateAsync(int id);
        Task<DailyActivity> ActivateAsync(int id);
        Task<List<DailyActivity>> ApproveActivitiesAsync(List<int> activityIds, int employeeId, int projectId, DateTime from, DateTime to, int approverId);
    }
}
