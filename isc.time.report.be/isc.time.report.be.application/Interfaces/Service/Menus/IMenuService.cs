using isc.time.report.be.domain.Models.Response.Menus;

namespace isc.time.report.be.application.Interfaces.Service.Menus
{
    public interface IMenuService
    {
        Task<List<GetAllUserMenusResponse>> GetMenusResponsesAsync(int UserId);
    }
}
