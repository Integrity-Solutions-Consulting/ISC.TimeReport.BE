using isc.time.report.be.domain.Entity.Auth;

namespace isc.time.report.be.application.Interfaces.Repository.Auth
{
    public interface IAuthRepository
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserAndRoleByUsername(string username);
        Task<User> CreateUser(User user, List<int> roleIds, string destinatarioCorreo, string htmlCorreo);
        Task UpdateUserLastLoginByID(int userId);
        Task<List<Role>> GetAllRolesByRolesID(List<int> RolesId);
        Task<Role?> GetRoleByNameAsync(string name);
        Task CreateRoleAsync(Role newRole);
        Task<List<Role>> GetAllRolesWithModulesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task UpdateRoleModulesAsync(Role role, List<int> newModuleIds);
        Task<User?> GetUserWithEmployeeAsync(string username);
        Task EnviarCorreoRecuperacionPasswordAsync(string username, string html);
        Task ResetPassword(int userId, string newPasswordHash);
    }
}
