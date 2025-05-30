using isc.time.report.be.domain.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.Users
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUsername(string username);
        Task<List<User>> GetAllUsers();
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
        Task<bool> AssignRolesToUser(int userId, List<int> rolIds);
        Task<bool> RemoveAllRolesFromUser(int userId);
    }
}
