using isc.time.report.be.domain.Entity.Catalogs;

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
