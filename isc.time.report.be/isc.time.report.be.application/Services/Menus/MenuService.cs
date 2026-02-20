using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.application.Interfaces.Service.Menus;
using isc.time.report.be.domain.Models.Response.Menus;

namespace isc.time.report.be.application.Services.Menus
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository menuRepository;
        public MenuService(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }
        public async Task<List<GetAllUserMenusResponse>> GetMenusResponsesAsync(int UserId)
        {
            var menus = await menuRepository.GetMenusByUserId(UserId);

            var result = menus.Select(m => new GetAllUserMenusResponse
            {
                ModuleName = m.ModuleName,
                ModulePath = m.ModulePath,
            }).ToList();

            return result;
        }

    }
}
