using isc.time.report.be.domain.Models.Response.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.Menus
{
    public interface IMenuService
    {
        Task<List<GetAllUserMenusResponse>> GetMenusResponsesAsync(int UserId);
    }
}
