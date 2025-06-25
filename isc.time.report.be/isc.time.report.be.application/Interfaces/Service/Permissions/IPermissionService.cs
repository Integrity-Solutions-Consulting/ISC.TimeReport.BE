using isc.time.report.be.domain.Models.Request.Permissions;
using isc.time.report.be.domain.Models.Response.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.Permissions
{
    public interface IPermissionService
    {
        Task<CreatePermissionResponse> CreatePermissionAsync(CreatePermissionRequest request, int employeeId);
        Task<GetPermissionResponse> ApprovePermissionAsync(PermissionAproveRequest request, ClaimsPrincipal user);
        Task<List<GetPermissionResponse>> GetAllPermissionAsync(ClaimsPrincipal user);
    }
}
