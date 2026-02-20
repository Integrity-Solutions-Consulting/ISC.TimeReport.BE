using isc.time.report.be.domain.Models.Request.Permissions;
using isc.time.report.be.domain.Models.Response.Permissions;
using System.Security.Claims;

namespace isc.time.report.be.application.Interfaces.Service.Permissions
{
    public interface IPermissionService
    {
        Task<CreatePermissionResponse> CreatePermissionAsync(CreatePermissionRequest request, int employeeId);
        Task<GetPermissionResponse> ApprovePermissionAsync(PermissionAproveRequest request, ClaimsPrincipal user);
        Task<List<GetPermissionResponse>> GetAllPermissionAsync(ClaimsPrincipal user);
    }
}
