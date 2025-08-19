using isc.time.report.be.domain.Entity.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.PermissionTypes
{
    public interface IPermissionTypeRepository
    {
        Task<List<PermissionType>> GetAllPermissionTypeAsync();
        Task<PermissionType?> GetPermissionTypeByIdAsync(int id);
        Task<PermissionType> CreatePermissionTypeAsync(PermissionType entity);
        Task<PermissionType> UpdatePermissionTypeAsync(PermissionType entity);
        Task<PermissionType> InactivatePermissionTypeAsync(int id);
        Task<PermissionType> ActivatePermissionTypeAsync(int id);
    }
}
