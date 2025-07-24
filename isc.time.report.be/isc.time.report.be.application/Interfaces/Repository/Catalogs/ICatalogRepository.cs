using isc.time.report.be.domain.Entity.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.Catalogs
{
    public interface ICatalogRepository
    {
        Task<List<ActivityType>> GetActivityTypeActivosAsync();
        Task<List<ApprovalStatus>> GetApprovalStatusActivosAsync();
        Task<List<Gender>> GetGenderActivosAsync();
        Task<List<Department>> GetDepartmentActivosAsync();
        Task<List<IdentificationType>> GetIdentificationTypeActivosAsync();
        Task<List<Nationality>> GetNationalityActivosAsync();
        Task<List<PermissionType>> GetPermissionTypeActivosAsync();
        Task<List<Position>> GetPositionActivosAsync();
        Task<List<ProjectStatus>> GetProjectStatusActivosAsync();
        Task<List<ProjectType>> GetProjectTypeActivosAsync();
        Task<List<WorkMode>> GetWorkModeActivosAsync();
    }
}
