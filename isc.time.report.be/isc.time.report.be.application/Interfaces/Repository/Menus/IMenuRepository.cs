using isc.time.report.be.domain.Entity.Modules;

namespace isc.time.report.be.application.Interfaces.Repository.Menus
{
    public interface IMenuRepository
    {
        Task<List<Module>> GetAllMenusAsync();
        Task<List<Module>> GetAllMenusDetailAsync();
        Task<Module?> GetMenuByIdAsync(int id);
        Task<Module> CreateMenuAsync(Module menu);
        Task<bool> UpdateMenuAsync(Module updatedMenu);
        Task<bool> InactivateMenuAsync(int id);
        Task<bool> ActivateMenuAsync(int id);
        Task<List<Module>> GetMenuByRolIdAsync(int rolId);
        Task<List<Module>> GetMenusByUserId(int UserId);
        Task<List<Module>> GetAllModulesByUserID(int userId);
    }
}
