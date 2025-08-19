using isc.time.report.be.application.Interfaces.Repository.Permissions;
using isc.time.report.be.application.Interfaces.Repository.PermissionTypes;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Exceptions;
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
            var list = await _context.PermissionTypes.ToListAsync();
            if (!list.Any())
            {
                throw new ServerFaultException("No se encontraron PermissionType");
            }
            return list;
        }

        public async Task<PermissionType?> GetPermissionTypeByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ClientFaultException("El ID es inválido.");
            }
            var permissionsType = await _context.PermissionTypes.FindAsync(id);
            if (permissionsType == null)
            {
                throw new ClientFaultException($"No se encontró el tipo de permiso con ID {id}.");
            }

            return permissionsType;
        }

        public async Task<PermissionType> CreatePermissionTypeAsync(PermissionType entity)
        {
            bool exists = await _context.PermissionTypes
             .AnyAsync(pt => pt.TypeCode == entity.TypeCode);

            if (exists)
                throw new ClientFaultException($"Ya existe un PermissionType con el TypeCode '{entity.TypeCode}'.");
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
            if (entity == null) throw new ClientFaultException("No encontrado");
            entity.Status = false;
            entity.ModificationDate = DateTime.Now;
            _context.PermissionTypes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<PermissionType> ActivatePermissionTypeAsync(int id)
        {
            var entity = await GetPermissionTypeByIdAsync(id);
            if (entity == null) throw new ClientFaultException("No encontrado");
            entity.Status = true;
            entity.ModificationDate = DateTime.Now;
            _context.PermissionTypes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
