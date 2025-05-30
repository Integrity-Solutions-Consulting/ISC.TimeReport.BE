using isc.time.report.be.domain.Entity.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.Menus
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetAllMenusAsync();
        Task<List<Menu>> GetAllMenusDetailAsync();
        Task<Menu?> GetMenuByIdAsync(int id);
        Task<Menu> CreateMenuAsync(Menu menu);
        Task<bool> UpdateMenuAsync(Menu updatedMenu);
        Task<bool> InactivateMenuAsync(int id);
        Task<bool> ActivateMenuAsync(int id);
        Task<List<Menu>> GetMenuByRolIdAsync(int rolId);
        Task<List<Menu>> GetMenusByUserId(int UserId);
    }
}
