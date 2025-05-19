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
        Task<User> GetUserByUsername(string username);
        Task<User> CreateUser(User user);
        Task UpdateUserLastLogin(int userId);
    }
}
