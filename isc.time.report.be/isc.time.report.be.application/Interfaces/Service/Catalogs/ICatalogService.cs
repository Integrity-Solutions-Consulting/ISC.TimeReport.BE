using isc.time.report.be.domain.Models.Response.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.Catalogs
{
    public interface ICatalogService
    {
        Task<List<GetActivityTypeResponse>> GetActivityTypesAsync();
        Task<List<GetApprovalStatusResponse>> GetApprovalStatusesAsync();
        Task<List<GetGenderResponse>> GetGendersAsync();
        Task<List<GetIdentificationTypeResponse>> GetIdentificationTypesAsync();
        Task<List<GetNationalityResponse>> GetNationalitiesAsync();
        Task<List<GetPermissionTypeResponse>> GetPermissionTypesAsync();
        Task<List<GetPositionResponse>> GetPositionsAsync();
        Task<List<GetProjectStatusResponse>> GetProjectStatusesAsync();
    }
}
