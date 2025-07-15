using isc.time.report.be.domain.Entity.Permisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.Permissions
{
    public interface IPermissionRepository
    {
        Task<Permission> CreateAsync(Permission permission);
        Task<Permission> ApproveAsync(Permission permission);
        Task<List<Permission>> GetAllAsync(int? employeeId, bool isAdmin);
        Task<Permission> GetPermissionByIdAsync(int id);
        Task<List<Permission>> GetPermissionsAprovedByEmployeeIdAsync(int employeeId);
    }
}
