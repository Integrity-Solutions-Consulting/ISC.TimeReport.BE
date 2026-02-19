using isc.time.report.be.domain.Models.Request.PermissionTypes;
using isc.time.report.be.domain.Models.Response.PermissionTypes;

namespace isc.time.report.be.application.Interfaces.Service.PermissionTypes
{
    public interface IPermissionTypeService
    {
        Task<List<GetPermissionTypeResponse>> GetAllPermissionTypesAsync();
        Task<GetPermissionTypeResponse> GetPermissionTypeByIdAsync(int id);
        Task<CreatePermissionTypeResponse> CreatePermissionTypeAsync(CreatePermissionTypeRequest request);
        Task<UpdatePermissionTypeResponse> UpdatePermissionTypeAsync(int id, UpdatePermissionTypeRequest request);
        Task<ActiveInactivePermissionTypeResponse> InactivatePermissionTypeAsync(int id);
        Task<ActiveInactivePermissionTypeResponse> ActivatePermissionTypeAsync(int id);
    }
}
