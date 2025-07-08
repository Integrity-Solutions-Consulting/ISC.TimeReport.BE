using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Auth;

namespace isc.time.report.be.application.Interfaces.Repository.Auth
{
    public interface IAuthRepository
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserAndRoleByUsername(string username);
        Task<User> CreateUser(User user, List<int> RolesId);
        Task UpdateUserLastLoginByID(int userId);
        Task<List<Role>> GetAllRolesByRolesID(List<int> RolesId);
        Task<Role?> GetRoleByNameAsync(string name);
        Task CreateRoleAsync(Role newRole);
        Task<List<Role>> GetAllRolesWithModulesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task UpdateRoleModulesAsync(Role role, List<int> newModuleIds);
    }
}
