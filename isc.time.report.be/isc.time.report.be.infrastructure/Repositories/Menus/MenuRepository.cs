using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
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
            return await _dbContext.Modules
                .Where(m => m.Status)
                .Select(m => new Module
                {
                    Id = m.Id,
                    ModuleName = m.ModuleName,
                    ModulePath = m.ModulePath
                })
                .ToListAsync();
        }

        public async Task<List<Module>> GetAllMenusDetailAsync()
        {
            return await _dbContext.Modules.ToListAsync();
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
                .Include(u => u.UserRole) // Incluye la tabla intermedia UserRole
                    .ThenInclude(ur => ur.Role) // Luego incluye el Role
                        .ThenInclude(r => r.RoleModule) // Luego incluye la tabla intermedia MenuRole
                            .ThenInclude(mr => mr.Module) // Finalmente incluye el Modules
                .SelectMany(u => u.UserRole) // Accede a los roles del usuario
                .Select(ur => ur.Role)      // Obtiene el objeto Role
                .SelectMany(r => r.RoleModule) // Accede a los MenuRoles de ese rol
                .Select(mr => mr.Module)      // Obtiene el objeto Modules
                .Distinct()                 // ¡Importante para evitar menús duplicados!
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

            var menus = await _dbContext.RoleModules
                .Where(mr => mr.RoleID == rolId)
                .Select(mr => mr.Module)
                .ToListAsync();

            return menus;
        }

        public async Task<List<Module>> GetMenusByUserId(int UserId)
        {
            var menus = await _dbContext.Users
                .Where(u => u.Id == UserId)
                .SelectMany(u => u.UserRole) // accedemos a los roles del usuario
                .SelectMany(ur => ur.Role.RoleModule) // accedemos a los menús de esos roles
                .Select(mr => mr.Module) // obtenemos el menú
                .Distinct() // evitamos duplicados
                .ToListAsync();

            return menus;
        }

    }
}

