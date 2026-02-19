using isc.time.report.be.domain.Models.Response.Catalogs;

namespace isc.time.report.be.application.Interfaces.Service.Catalogs
{
    public interface ICatalogService
    {
        Task<List<GetActivityTypeResponse>> GetActivityTypesAsync();
        Task<List<GetApprovalStatusResponse>> GetApprovalStatusesAsync();
        Task<List<GetDepartmentResponse>> GetDepartmentsAsync();
        Task<List<GetGenderResponse>> GetGendersAsync();
        Task<List<GetIdentificationTypeResponse>> GetIdentificationTypesAsync();
        Task<List<GetNationalityResponse>> GetNationalitiesAsync();
        Task<List<GetPermissionTypeResponse>> GetPermissionTypesAsync();
        Task<List<GetPositionResponse>> GetPositionsAsync();
        Task<List<GetProjectStatusResponse>> GetProjectStatusesAsync();
        Task<List<GetProjectTypeResponse>> GetProjectTypesAsync();
        Task<List<GetWorkModeResponse>> GetWorkModesAsync();
        Task<List<GetCompanyCatalogResponse>> GetCompanyCatalogAsync();
        Task<List<GetEmployeeCategoryResponse>> GetEmployeeCategoryAsync();
    }
}
