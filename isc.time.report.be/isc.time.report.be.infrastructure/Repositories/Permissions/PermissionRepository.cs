using isc.time.report.be.application.Interfaces.Repository.Permissions;
using isc.time.report.be.domain.Entity.Permisions;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Permissions
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DBContext _dbContext;

        public PermissionRepository(DBContext context)
        {
            _dbContext = context;
        }

        public async Task<Permission> CreateAsync(Permission permission)
        {
            await _dbContext.Permissions.AddAsync(permission);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ServerFaultException(
                    message: "Error al guardar cambios en la base de datos.",
                    code: 500,
                    innerException: ex
                );
            }
            return permission;
        }

        public async Task<Permission> ApproveAsync(Permission permission)
        {
            _dbContext.Permissions.Update(permission);
            await _dbContext.SaveChangesAsync();
            return permission;
        }

        public async Task<List<Permission>> GetAllAsync(int? employeeId, bool isAdmin)
        {
            var query = _dbContext.Permissions.AsQueryable();
            if (!isAdmin && employeeId.HasValue)
            {
                query = query.Where(p => p.EmployeeID == employeeId);
            }
            var permissions = await query.ToListAsync();

            //if (!permissions.Any())
            //{
            //    throw new ClientFaultException("No se encontraron permisos.");
            //}
            return permissions;
        }

        public async Task<Permission> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ClientFaultException("El ID no puede ser negativo");
            }
            var permission = await _dbContext.Permissions
                .FirstOrDefaultAsync(p => p.Id == id);

            if (permission == null)
                throw new KeyNotFoundException($"No se encontró el permiso con ID {id}");

            return permission;
        }

        public async Task<List<Permission>> GetPermissionsAprovedByEmployeeIdAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ClientFaultException("El ID del empleado es inválido.");
            }
            var permissions = await _dbContext.Permissions
                .Where(p => p.EmployeeID == employeeId && p.ApprovalStatusID == 2)
                .ToListAsync();
            if (!permissions.Any())
            {
                throw new ServerFaultException($"No se encontraron permisos aprobados para el empleado con ID {employeeId}.");
            }
            return permissions;
        }
    }
}
