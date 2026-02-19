using isc.time.report.be.domain.Models.Request.Auth;
using isc.time.report.be.domain.Models.Response.Auth;
using isc.time.report.be.domain.Models.Response.Users;

namespace isc.time.report.be.application.Interfaces.Service.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest registerRequest);
        Task<RegisterResponse> Register(RegisterRequest registerRequest);
        Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);
        Task<List<GetRolesResponse>> GetAllRolesAsync();
        Task<RoleResponse> UpdateRoleAsync(int id, UpdateRoleRequest request);
        Task RecuperarPasswordAsync(string username);
        Task ResetPasswordWithToken(string token, ResetPasswordRequest request);
        Task ChangePasswordWithToken(string token, ChangePasswordRequest request);
    }
}
