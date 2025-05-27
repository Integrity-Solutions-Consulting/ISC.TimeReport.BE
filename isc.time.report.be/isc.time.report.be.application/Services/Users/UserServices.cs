using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Users;
using isc.time.report.be.application.Interfaces.Service.Users;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Models.Request.Users;
using isc.time.report.be.domain.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.Users
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository userRepository;
        private readonly PasswordUtils passwordUtils;
        public UserServices(IUserRepository userRepository, PasswordUtils passwordUtils)
        {
            this.userRepository = userRepository;
            this.passwordUtils = passwordUtils;
        }

        public async Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            var user = await userRepository.GetUserById(id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            user.Username = request.Username;
            user.Password = passwordUtils.HashPassword(request.Password);

            var updatedUser = await userRepository.UpdateUser(user);

            return new UserResponse
            {
                Username = updatedUser.Username,
                Password = updatedUser.Password
            };
        }

        public async Task<UserResponse> SuspendUser(int id)
        {
            var user = await userRepository.GetUserById(id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            user.Status = false;

            var updatedUser = await userRepository.UpdateUser(user);

            return new UserResponse
            {
                Username = updatedUser.Username,
            };
        }

        public async Task<UserResponse> UnSuspendUser(int id)
        {
            var user = await userRepository.GetUserById(id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            user.Status = true;

            var updatedUser = await userRepository.UpdateUser(user);

            return new UserResponse
            {
                Username = updatedUser.Username,
            };
        }

        public async Task<UserResponse> UpdatePassword(int id, UpdatePasswordRequest request)
        {
            var user = await userRepository.GetUserById(id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            user.Password = passwordUtils.HashPassword(request.Password);

            var updatedUser = await userRepository.UpdateUser(user);

            return new UserResponse
            {
                Username = updatedUser.Username,
                Password = updatedUser.Password
            };
        }

        public async Task<IEnumerable<GetAllUsersResponse>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllUsers();

            var result = users.Select(user => new GetAllUsersResponse
            {
                Id = user.Id,
                Username = user.Username,
                Status = user.Status,
                Roles = user.UsersRols.Select(ur => ur.Rols.RolName).ToList()
            });

            return result;
        }

    }
}
