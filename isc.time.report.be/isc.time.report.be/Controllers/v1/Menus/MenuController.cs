using isc.time.report.be.application.Interfaces.Service.Auth;
using isc.time.report.be.application.Interfaces.Service.Menus;
using isc.time.report.be.application.Services.Auth;
using isc.time.report.be.application.Services.Menus;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Response.Auth;
using isc.time.report.be.domain.Models.Response.Menus;
using isc.time.report.be.domain.Models.Response.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Menus
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {

        private readonly IMenuService menuService;
        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [HttpGet("GetAllMenusById/{userId}")]
        public async Task<ActionResult<SuccessResponse<List<GetAllUserMenusResponse>>>> GetAllUserMenuById(int userId)
        {
            var menusResponse = await menuService.GetMenusResponsesAsync(userId);

            if (menusResponse == null || menusResponse.Count == 0)
            {
                return Ok(new SuccessResponse<List<GetAllUserMenusResponse>>
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Data = menusResponse
                });
            }

            return Ok(new SuccessResponse<List<GetAllUserMenusResponse>>
            {
                TraceId = HttpContext.TraceIdentifier,
                Data = menusResponse
            });
        }




    }
}
