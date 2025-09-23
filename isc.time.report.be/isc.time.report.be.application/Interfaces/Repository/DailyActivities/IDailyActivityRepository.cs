using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Response.DailyActivities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.DailyActivities
{
    public interface IDailyActivityRepository
    {
        Task<List<DailyActivity>> GetAllAsync(int employeeId, int? month, int? year);
        Task<DailyActivity?> GetByIdAsync(int id);
        Task<DailyActivity> CreateAsync(DailyActivity entity);
        Task<DailyActivity> UpdateAsync(DailyActivity entity);
        Task<DailyActivity> InactivateAsync(int id);
        Task<DailyActivity> ActivateAsync(int id);
        Task ApproveActivitiesAsync(List<int> activityIds,int employeeId,int projectId, DateTime from,DateTime to, int approverId);
        Task AddRangeAsync(List<DailyActivity> activities);
        Task<string?> GetActivityTypeNameByIdAsync(int activityTypeId);
        Task<List<DailyActivity>> GetActivitiesForApprovalAsync(List<int> activityIds, int employeeId, int projectId, DateTime from, DateTime to);
        Task<bool> ExistsApprovedActivitiesAsync(int employeeId, int month, int year);
        
        }
}
