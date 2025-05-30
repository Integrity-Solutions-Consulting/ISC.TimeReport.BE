using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.application.Interfaces.Repository.Users;
using isc.time.report.be.application.Interfaces.Service.Menus;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Models.Response.Menus;
using isc.time.report.be.domain.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                NombreMenu = m.NombreMenu,
                RutaMenu = m.RutaMenu,
            }).ToList();

            return result;
        }

    }
}
