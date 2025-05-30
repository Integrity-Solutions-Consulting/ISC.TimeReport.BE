using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Menu;
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

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            return await _dbContext.Menu
                .Where(m => m.Status)
                .Select(m => new Menu
                {
                    Id = m.Id,
                    NombreMenu = m.NombreMenu,
                    RutaMenu = m.RutaMenu
                })
                .ToListAsync();
        }

        public async Task<List<Menu>> GetAllMenusDetailAsync()
        {
            return await _dbContext.Menu.ToListAsync();
        }

        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await _dbContext.Menu
                .Where(m => m.Id == id)
                .Select(m => new Menu
                {
                    Id = m.Id,
                    NombreMenu = m.NombreMenu,
                    RutaMenu = m.RutaMenu
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Menu> CreateMenuAsync(Menu menu)
        {

            menu.CreatedAt = DateTime.Now;
            menu.UpdatedAt = null;
            menu.Status = true;
            await _dbContext.Menu.AddAsync(menu);

            _dbContext.Menu.Add(menu);
            await _dbContext.SaveChangesAsync();
            return menu;
        }

        public async Task<bool> UpdateMenuAsync(Menu updatedMenu)
        {
            var existingMenu = await _dbContext.Menu.FindAsync(updatedMenu.Id);
            if (existingMenu == null) return false;

            existingMenu.NombreMenu = updatedMenu.NombreMenu;
            existingMenu.RutaMenu = updatedMenu.RutaMenu;
            existingMenu.CreatedAt = DateTime.UtcNow;
            existingMenu.UpdatedAt = updatedMenu.UpdatedAt;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InactivateMenuAsync(int id)
        {
            var menu = await _dbContext.Menu.FindAsync(id);
            if (menu == null) return false;

            menu.Status = false;
            menu.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateMenuAsync(int id)
        {
            var menu = await _dbContext.Menu.FindAsync(id);
            if (menu == null) return false;

            menu.Status = true;
            menu.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Menu>> GetMenuByRolIdAsync(int rolId)
        {

            var menus = await _dbContext.Menu_Rols
                .Where(mr => mr.RolsId == rolId)
                .Select(mr => mr.Menu)
                .ToListAsync();

            return menus;
        }

        public async Task<List<Menu>> GetMenusByUserId(int UserId)
        {
            var menus = await _dbContext.Users
                .Where(u => u.Id == UserId)
                .SelectMany(u => u.UsersRols) // accedemos a los roles del usuario
                .SelectMany(ur => ur.Rols.MenuRols) // accedemos a los menús de esos roles
                .Select(mr => mr.Menu) // obtenemos el menú
                .Distinct() // evitamos duplicados
                .ToListAsync();

            return menus;
        }

    }
}

