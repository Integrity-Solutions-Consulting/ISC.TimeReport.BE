using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Users;
using isc.time.report.be.application.Interfaces.Service.Users;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Users;
using isc.time.report.be.domain.Models.Response.Users;

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

        public async Task<string> UpdatePassword(int userId, UpdatePasswordRequest request)
        {
            var user = await userRepository.GetUserById(userId);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            var isValidPassword = passwordUtils.VerifyPassword(request.OldPassword, user.PasswordHash);
            if (!isValidPassword)
                throw new ClientFaultException("La contraseña actual es incorrecta.", 400);

            if (request.NewPassword != request.ConfirmPassword)
                throw new ClientFaultException("La nueva contraseña y su confirmación no coinciden.", 400);

            user.PasswordHash = passwordUtils.HashPassword(request.NewPassword);
            user.MustChangePassword = false;

            var updatedUser = await userRepository.UpdateUser(user);

            return "Contraseña cambiada correctamente";
        }
        /// <summary>
        /// SEUSAAA
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetAllUsersResponse>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllUsers();

            var result = new List<GetAllUsersResponse>();

            foreach (var user in users)
            {
                var roles = user.UserRole?
                    .Where(ur => ur.Status && ur.Role != null)
                    .Select(ur => new RoleResponse
                    {
                        Id = ur.Role.Id,
                        RoleName = ur.Role.RoleName
                    })
                    .ToList() ?? new List<RoleResponse>();

                var userModules = user.UserModule?
                    .Where(um => um.Status && um.Module != null)
                    .Select(um => um.Module)
                    .ToList() ?? new List<Module>();

                var roleModules = user.UserRole?
                    .Where(ur => ur.Status && ur.Role != null)
                    .SelectMany(ur => ur.Role.RoleModule
                        .Where(rm => rm.Status && rm.Module != null)
                        .Select(rm => rm.Module))
                    .ToList() ?? new List<Module>();

                var allModules = userModules
                    .Concat(roleModules)
                    .GroupBy(m => m.Id)
                    .Select(g => g.First())
                    .ToList();

                var moduleResponses = allModules.Select(m => new ModuleResponse
                {
                    Id = m.Id,
                    ModuleName = m.ModuleName,
                    ModulePath = m.ModulePath,
                    Icon = m.Icon,
                    DisplayOrder = m.DisplayOrder
                }).ToList();

                result.Add(new GetAllUsersResponse
                {
                    Id = user.Id,
                    EmployeeID = user.EmployeeID,
                    Username = user.Username,
                    IsActive = user.IsActive,
                    Role = roles,
                    Modules = moduleResponses
                });
            }

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

        public async Task<GetRolesOfUserResponse> GetRolesOfUser(int userId)
        {
            var user = await userRepository.GetUserById(userId);
            if (user == null)
                throw new ClientFaultException("Usuario no encontrado", 404);

            if (user.UserRole == null || !user.UserRole.Any(r => r.Status))
                throw new ClientFaultException("El usuario no tiene roles asignados", 404);

            var roles = user.UserRole
                .Where(r => r.Status && r.Role != null)
                .Select(r => new RoleResponse
                {
                    Id = r.Role.Id,
                    RoleName = r.Role.RoleName
                })
                .ToList();

            return new GetRolesOfUserResponse
            {
                Id = user.Id,
                EmployeeID = user.EmployeeID,
                Username = user.Username,
                IsActive = user.IsActive,
                Role = roles
            };
        }

        public async Task<GetModulesOfUserResponse> GetModulesOfUser(int userId)
        {
            var user = await userRepository.GetUserById(userId);
            if (user == null)
                throw new ClientFaultException("Usuario no encontrado", 404);

            var directModules = user.UserModule?
                .Where(m => m.Status && m.Module != null)
                .Select(m => m.Module)
                .ToList() ?? new List<Module>();

            var roleModules = user.UserRole?
                .Where(ur => ur.Status && ur.Role != null)
                .SelectMany(ur => ur.Role.RoleModule
                    .Where(rm => rm.Status && rm.Module != null)
                    .Select(rm => rm.Module))
                .ToList() ?? new List<Module>();

            var allModules = directModules
                .Concat(roleModules)
                .GroupBy(m => m.Id)
                .Select(g => g.First()) // Elimina duplicados por ID
                .ToList();

            if (!allModules.Any())
                throw new ClientFaultException("El usuario no tiene módulos asignados", 404);

            var responseModules = allModules.Select(m => new ModuleResponse
            {
                Id = m.Id,
                ModuleName = m.ModuleName,
                ModulePath = m.ModulePath,
                Icon = m.Icon,
                DisplayOrder = m.DisplayOrder
            }).ToList();

            return new GetModulesOfUserResponse
            {
                Id = user.Id,
                EmployeeID = user.EmployeeID,
                Username = user.Username,
                IsActive = user.IsActive,
                Modules = responseModules
            };
        }

    }
}
