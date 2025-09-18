using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Models.Request.DailyActivities;
using isc.time.report.be.domain.Models.Response.DailyActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.DailyActivities
{
    public interface IDailyActivityService
    {
        Task<List<GetDailyActivityResponse>> GetAllAsync();
        Task<GetDailyActivityResponse> GetByIdAsync(int id);
        Task<CreateDailyActivityResponse> CreateAsync(CreateDailyActivityRequest request, int employeeId, int id);
        Task<UpdateDailyActivityResponse> UpdateAsync(int id, UpdateDailyActivityRequest request, int employeeId);
        Task<ActiveInactiveDailyActivityResponse> InactivateAsync(int id);
        Task<ActiveInactiveDailyActivityResponse> ActivateAsync(int id);
        Task<List<GetDailyActivityResponse>> ApproveActivitiesAsync(AproveDailyActivityRequest request, int approverId);
        Task<CreateListOfDailyActivityFromBG> ImportActivitiesAsync(List<CreateDailyActivityFromBGResponse> excelRows);
        Task<List<CreateDailyActivityFromBGResponse>> ReadActivitiesFromExcelAsync(Stream fileStream);
    }
}
