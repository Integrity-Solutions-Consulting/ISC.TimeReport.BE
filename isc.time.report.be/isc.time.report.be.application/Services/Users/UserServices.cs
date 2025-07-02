using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Users;
using isc.time.report.be.application.Interfaces.Service.Users;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Models.Request.Users;
using isc.time.report.be.domain.Models.Response.Users;
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
        private readonly IMapper _mapper;
        public UserServices(IUserRepository userRepository, PasswordUtils passwordUtils, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.passwordUtils = passwordUtils;
            this._mapper = mapper;
        }
        /// <summary>
        /// SE USAAAA
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            var user = await userRepository.GetUserById(id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            user.Username = request.username;
            user.PasswordHash = passwordUtils.HashPassword(request.Password);
            user.EmployeeID = request.EmployeeID;

            var updatedUser = await userRepository.UpdateUser(user);

            return new UserResponse
            {
                email = updatedUser.Username,
                Password = updatedUser.PasswordHash,
                EmployeeID = updatedUser.EmployeeID
            };
        }
        /// <summary>
        /// se USAAA
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserResponse> SuspendUser(int id)
        {
            var user = await userRepository.GetUserById(id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            user.Status = false;

            var updatedUser = await userRepository.UpdateUser(user);

            return new UserResponse
            {
                email = updatedUser.Username,
            };
        }
        /// <summary>
        /// USAAAA
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserResponse> UnSuspendUser(int id)
        {
            var user = await userRepository.GetUserById(id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            user.Status = true;

            var updatedUser = await userRepository.UpdateUser(user);

            return new UserResponse
            {
                email = updatedUser.Username,
            };
        }
        /// <summary>
        /// SE USA PA UPDATE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserResponse> UpdatePassword(int id, UpdatePasswordRequest request)
        {
            var user = await userRepository.GetUserById(id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            user.PasswordHash = passwordUtils.HashPassword(request.Password);

            var updatedUser = await userRepository.UpdateUser(user);

            return new UserResponse
            {
                email = updatedUser.Username,
                Password = updatedUser.PasswordHash
            };
        }
        /// <summary>
        /// SEUSAAA
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetAllUsersResponse>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllUsers();

            var result = _mapper.Map<List<GetAllUsersResponse>>(users);

            return result;
        }



        public async Task AssignRolesToUser(AssignRolesToUserRequest request)
        {
            var existing = await userRepository.GetUserRolesAsync(request.UserID);
            var finalList = new List<UserRole>();
            var now = DateTime.Now;

            foreach (var roleId in request.RolesIDs)
            {
                var match = existing.FirstOrDefault(e => e.RoleID == roleId);

                if (match == null)
                {
                    finalList.Add(new UserRole
                    {
                        UserID = request.UserID,
                        RoleID = roleId,
                        Status = true,
                        CreationDate = now,
                        CreationUser = "SYSTEM"
                    });
                }
                else if (!match.Status)
                {
                    match.Status = true;
                    match.ModificationDate = now;
                    match.ModificationUser = "SYSTEM";
                    finalList.Add(match);
                }
                else
                {
                    finalList.Add(match);
                }
            }

            foreach (var ur in existing)
            {
                if (!request.RolesIDs.Contains(ur.RoleID) && ur.Status)
                {
                    ur.Status = false;
                    ur.ModificationDate = now;
                    ur.ModificationUser = "SYSTEM";
                    finalList.Add(ur);
                }
            }

            await userRepository.SaveUserRolesAsync(finalList);
        }

        public async Task AssignModulesToUser(AssignModuleToUserRequest request)
        {
            var existing = await userRepository.GetUserModulesAsync(request.UserID);
            var finalList = new List<UserModule>();
            var now = DateTime.Now;

            foreach (var moduleId in request.ModuleIDs)
            {
                var match = existing.FirstOrDefault(e => e.ModuleID == moduleId);

                if (match == null)
                {
                    finalList.Add(new UserModule
                    {
                        UserID = request.UserID,
                        ModuleID = moduleId,
                        Status = true,
                        CanView = true,
                        CreationDate = now,
                        CreationUser = "SYSTEM"
                    });
                }
                else if (!match.Status)
                {
                    match.Status = true;
                    match.ModificationDate = now;
                    match.ModificationUser = "SYSTEM";
                    finalList.Add(match);
                }
                else
                {
                    finalList.Add(match);
                }
            }

            foreach (var um in existing)
            {
                if (!request.ModuleIDs.Contains(um.ModuleID) && um.Status)
                {
                    um.Status = false;
                    um.ModificationDate = now;
                    um.ModificationUser = "SYSTEM";
                    finalList.Add(um);
                }
            }

            await userRepository.SaveUserModulesAsync(finalList);
        }

    }
}
