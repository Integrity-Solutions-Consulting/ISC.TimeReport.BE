using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Menus
{
    public class MenuRepository : IMenuRepository
    {

        private readonly DBContext _dbContext;
        public MenuRepository(DBContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<List<Module>> GetAllMenusAsync()
        {
            var modules = await _dbContext.Modules
                .Where(m => m.Status)
                .Select(m => new Module
                {
                    Id = m.Id,
                    ModuleName = m.ModuleName,
                    ModulePath = m.ModulePath
                })
                .ToListAsync();
            if (!modules.Any())
            {
                throw new ServerFaultException("No existe ningun modulo");
            }
            return modules;
        }

        public async Task<List<Module>> GetAllMenusDetailAsync()
        {
            var modules = await _dbContext.Modules.ToListAsync();
            if (!modules.Any())
            {
                throw new ServerFaultException("No existe ningun Menus Detail");
            }
            return modules;
        }
        /// <summary>
        /// ESTE SI HACE COSAS
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Module>> GetAllModulesByUserID(int userId)
        {
            return await _dbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.UserRole
                    .Where(ur => ur.Status) // ✅ solo roles activos
                    .SelectMany(ur => ur.Role.RoleModule
                        .Where(rm => rm.Status) // ✅ solo relaciones Role-Module activas
                        .Select(rm => rm.Module)))
                .Distinct() // evitar duplicados
                .Select(m => new Module
                {
                    Id = m.Id,
                    ModuleName = m.ModuleName,
                    ModulePath = m.ModulePath,
                    Icon = m.Icon,
                    DisplayOrder = m.DisplayOrder,
                })
                .ToListAsync();
        }

        public async Task<Module?> GetMenuByIdAsync(int id)
        {
            return await _dbContext.Modules
                .Where(m => m.Id == id)
                .Select(m => new Module
                {
                    Id = m.Id,
                    ModuleName = m.ModuleName,
                    ModulePath = m.ModulePath
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Module> CreateMenuAsync(Module menu)
        {

            menu.CreationDate = DateTime.Now;
            menu.ModificationDate = null;
            menu.Status = true;
            await _dbContext.Modules.AddAsync(menu);

            _dbContext.Modules.Add(menu);
            await _dbContext.SaveChangesAsync();
            return menu;
        }

        public async Task<bool> UpdateMenuAsync(Module updatedMenu)
        {
            var existingMenu = await _dbContext.Modules.FindAsync(updatedMenu.Id);
            if (existingMenu == null) return false;

            existingMenu.ModuleName = updatedMenu.ModuleName;
            existingMenu.ModulePath = updatedMenu.ModulePath;
            existingMenu.CreationDate = DateTime.UtcNow;
            existingMenu.ModificationDate = updatedMenu.ModificationDate;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InactivateMenuAsync(int id)
        {
            var menu = await _dbContext.Modules.FindAsync(id);
            if (menu == null) return false;

            menu.Status = false;
            menu.ModificationDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateMenuAsync(int id)
        {
            var menu = await _dbContext.Modules.FindAsync(id);
            if (menu == null) return false;

            menu.Status = true;
            menu.ModificationDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Module>> GetMenuByRolIdAsync(int rolId)
        {
            if(rolId <= 0)
            {
                throw new ClientFaultException("El ID de rol no puede ser negativo");
            }
            var menus = await _dbContext.RoleModules
                .Where(mr => mr.RoleID == rolId)
                .Select(mr => mr.Module)
                .ToListAsync();
            if(!menus.Any())
            {
                throw new ServerFaultException($"No se encontró un Rol con ID {rolId}.");
            }

            return menus;
        }

        public async Task<List<Module>> GetMenusByUserId(int UserId)
        {
            if (UserId <= 0)
            {
                throw new ClientFaultException("El ID del usuario no puede ser negativo");
            }
            var menus = await _dbContext.Users
                .Where(u => u.Id == UserId)
                .SelectMany(u => u.UserRole) // accedemos a los roles del usuario
                .SelectMany(ur => ur.Role.RoleModule) // accedemos a los menús de esos roles
                .Select(mr => mr.Module) // obtenemos el menú
                .Distinct() // evitamos duplicados
                .ToListAsync();
            if (!menus.Any())
            {
                throw new ServerFaultException($"No se encontró un Usuario con ID {UserId}.");
            }
            return menus;
        }

    }
}

