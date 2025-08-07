using isc.time.report.be.application.Interfaces.Service.Auth;
using isc.time.report.be.domain.Models.Request.Auth;
using isc.time.report.be.domain.Models.Response.Auth;
using isc.time.report.be.domain.Models.Response.Auth;
using isc.time.report.be.domain.Models.Response.Shared;
using isc.time.report.be.domain.Models.Response.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Auth
{
    [ApiExplorerSettings(GroupName = "v1")] 
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<SuccessResponse<LoginResponse>>> Login(LoginRequest loginRequest)
        {
            var login = await authService.Login(loginRequest);

            return Ok(new SuccessResponse<LoginResponse>(200, "Operacion Exitosa.", login));
        }

        [HttpPost("register")]
        //[Authorize(Roles = "Administrador")]
        public async Task<ActionResult<SuccessResponse<RegisterResponse>>> Register(RegisterRequest registerRequest)
        {
            var register = await authService.Register(registerRequest);

            return Ok(new SuccessResponse<RegisterResponse>(200, "Operacion Exitosa.", register));
        }

        [HttpPost("roles")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<SuccessResponse<RoleResponse>>> CreateRole([FromBody] CreateRoleRequest request)
        {
            var role = await authService.CreateRoleAsync(request);
            return Ok(new SuccessResponse<RoleResponse>(200, "Rol creado exitosamente.", role));
        }

        [HttpGet("GetRoles")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<SuccessResponse<List<GetRolesResponse>>>> GetAllRoles()
        {
            var roles = await authService.GetAllRolesAsync();
            return Ok(new SuccessResponse<List<GetRolesResponse>>(200, "Operación exitosa.", roles));
        }

        [HttpPut("UpdateRole/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<SuccessResponse<RoleResponse>>> UpdateRole(int id, [FromBody] UpdateRoleRequest request)
        {
            var role = await authService.UpdateRoleAsync(id, request);
            return Ok(new SuccessResponse<RoleResponse>(200, "Rol actualizado correctamente.", role));
        }

        [AllowAnonymous]
        [HttpPost("recuperar-password")]
        public async Task<IActionResult> RecuperarPassword([FromBody] RecuperarPasswordRequest request)
        {
            await authService.RecuperarPasswordAsync(request.Username);
            return Ok(new { message = "Si el usuario existe, se ha enviado un enlace de recuperación al correo registrado." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromBody] ResetPasswordRequest request)
        {
            await authService.ResetPasswordWithToken(token, request);
            return Ok("Contraseña restablecida correctamente.");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromQuery] string token, [FromBody] ChangePasswordRequest request)
        {
            await authService.ChangePasswordWithToken(token, request);
            return Ok("Contraseña restablecida correctamente.");
        }
    }
}
