using isc.time.report.be.application.Interfaces.Service.Auth;
using isc.time.report.be.application.Interfaces.Service.Users;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Response.Users;
using isc.time.report.be.domain.Models.Response.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Users
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserServices userService;
        public UserController(IUserServices userService)
        {
            this.userService = userService;
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<ActionResult<SuccessResponse<UserResponse>>> Update(int id, [FromBody] UpdateUserRequest request)
        {
            var updatedUser = await userService.UpdateUserAsync(id, request);
            return Ok(new SuccessResponse<UserResponse>
            {
                TraceId = HttpContext.TraceIdentifier,
                Data = updatedUser
            });
        }

        [HttpPut("SuspendUser/{id}")]
        public async Task<ActionResult<SuccessResponse<UserResponse>>> Suspend(int id)
        {
            var updatedUser = await userService.SuspendUser(id);
            return Ok(new SuccessResponse<UserResponse>
            {
                TraceId = HttpContext.TraceIdentifier,
                Data = updatedUser
            });
        }

        [HttpPut("UnSuspendUser/{id}")]
        public async Task<ActionResult<SuccessResponse<UserResponse>>> UnSuspend(int id)
        {
            var updatedUser = await userService.UnSuspendUser(id);
            return Ok(new SuccessResponse<UserResponse>
            {
                TraceId = HttpContext.TraceIdentifier,
                Data = updatedUser
            });
        }

        [HttpPut("ChangePassword/{id}")]
        public async Task<ActionResult<SuccessResponse<UserResponse>>> ChangePasswordById(int id, [FromBody] UpdatePasswordRequest request)
        {
            var updatedPassword = await userService.UpdatePassword(id, request);
            return Ok(new SuccessResponse<UserResponse>
            {
                TraceId = HttpContext.TraceIdentifier,
                Data = updatedPassword
            });
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<GetAllUsersResponse>>>> GetAllUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(new SuccessResponse<IEnumerable<GetAllUsersResponse>>
            {
                TraceId = HttpContext.TraceIdentifier,
                Data = users
            });
        }

    }
}
