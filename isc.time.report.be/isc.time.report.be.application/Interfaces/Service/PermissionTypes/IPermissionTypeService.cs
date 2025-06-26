using isc.time.report.be.domain.Models.Request.PermissionTypes;
using isc.time.report.be.domain.Models.Response.PermissionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
