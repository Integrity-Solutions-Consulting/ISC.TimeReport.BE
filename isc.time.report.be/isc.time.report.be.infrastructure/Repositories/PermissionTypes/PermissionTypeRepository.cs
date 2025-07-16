using isc.time.report.be.application.Interfaces.Repository.Permissions;
using isc.time.report.be.application.Interfaces.Repository.PermissionTypes;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.PermissionTypes
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private readonly DBContext _context;

        public PermissionTypeRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionType>> GetAllPermissionTypeAsync()
        {
            return await _context.PermissionTypes.ToListAsync();
        }

        public async Task<PermissionType?> GetPermissionTypeByIdAsync(int id)
        {
            return await _context.PermissionTypes.FindAsync(id);
        }

        public async Task<PermissionType> CreatePermissionTypeAsync(PermissionType entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.CreationUser = "SYSTEM";
            entity.Status = true;
            await _context.PermissionTypes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<PermissionType> UpdatePermissionTypeAsync(PermissionType entity)
        {
            entity.ModificationDate = DateTime.Now;
            entity.ModificationUser = "SYSTEM";
            _context.PermissionTypes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<PermissionType> InactivatePermissionTypeAsync(int id)
        {
            var entity = await GetPermissionTypeByIdAsync(id);
            if (entity == null) throw new Exception("No encontrado");
            entity.Status = false;
            entity.ModificationDate = DateTime.Now;
            _context.PermissionTypes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<PermissionType> ActivatePermissionTypeAsync(int id)
        {
            var entity = await GetPermissionTypeByIdAsync(id);
            if (entity == null) throw new Exception("No encontrado");
            entity.Status = true;
            entity.ModificationDate = DateTime.Now;
            _context.PermissionTypes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
