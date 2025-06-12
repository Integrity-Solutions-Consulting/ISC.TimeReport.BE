using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.application.Interfaces.Service.Auth;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Auth;
using isc.time.report.be.domain.Models.Response.Auth;
using isc.time.report.be.domain.Models.Response.Menus;
using isc.time.report.be.domain.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepository;
        private readonly IMenuRepository menuRepository;
        private readonly PasswordUtils passwordUtils;
        private readonly JWTUtils jwtUtils;
        public AuthService(IAuthRepository authRepository, PasswordUtils passwordUtils, JWTUtils jwtUtils, IMenuRepository menuRepository)
        {
            this.authRepository = authRepository;
            this.passwordUtils = passwordUtils;
            this.jwtUtils = jwtUtils;
            this.menuRepository = menuRepository;
        }
        /// <summary>
        /// SI SE ESTA USANDO
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        /// <exception cref="ClientFaultException"></exception>
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {

            if (loginRequest.Username == "string" || loginRequest.Password == "string")
            {
                throw new ClientFaultException("Complete los campos faltantes.", 401);
            }

            if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                throw new ClientFaultException("Complete los campos faltantes.", 401);
            }

            var user = await authRepository.GetUserAndRoleByUsername(loginRequest.Username);

            if (user == null)
            {
                throw new ClientFaultException("Usuario o contraseña incorrectos.", 401);
            }

            if (!passwordUtils.VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                throw new ClientFaultException("Usuario o contraseña incorrectos.", 401);
            }

            var accessibleBaseModule = await menuRepository.GetAllModulesByUserID(user.Id);

            await authRepository.UpdateUserLastLoginByID(user.Id);

            var userRoles = user.UserRole?.Select(ur => ur.Role)
                                            .Select(r => new RoleResponse
                                            {
                                                Id = r.Id,
                                                RoleName = r.RoleName
                                            })
                                            .ToList() ?? new List<RoleResponse>();

            var accessibleModules = accessibleBaseModule.Select(m => new ModuleResponse
            {
                Id = m.Id,
                ModuleName = m.ModuleName,
                ModulePath = m.ModulePath,
                Icon = m.Icon,
                DisplayOrder = m.DisplayOrder,

            }).ToList() ?? new List<ModuleResponse>();

            return new LoginResponse
            {
                UserID = user.Id,
                EmployeeID = user.EmployeeID,
                TOKEN = jwtUtils.GenerateToken(user),
                Roles = userRoles,
                Modules = accessibleModules
            };
        }

        public async Task<RegisterResponse> Register(RegisterRequest registerRequest)
        {

            if (registerRequest.Username == "string" || registerRequest.Password == "string")
            {
                throw new ClientFaultException("Complete los campos faltantes.", 401);
            }

            if (string.IsNullOrWhiteSpace(registerRequest.Username) || string.IsNullOrWhiteSpace(registerRequest.Password))
            {
                throw new ClientFaultException("Complete los campos faltantes.", 401);
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(registerRequest.Username))
            {
                throw new ClientFaultException("Ingrese un correo electrónico válido.", 401);
            }

            if (registerRequest.Password.Length < 8)
            {
                throw new ClientFaultException("La contraseña debe tener al menos 8 caracteres.", 401);
            }

            var user = await authRepository.GetUserAndRoleByUsername(registerRequest.Username);
            if (user != null)
            {
                throw new ClientFaultException("El nombre de usuario no está disponible.", 401);
            }

            var userNew = await authRepository.CreateUser(new User
            {
                EmployeeID = registerRequest.EmployeeID,
                Username = registerRequest.Username,
                PasswordHash = passwordUtils.HashPassword(registerRequest.Password),
                IsActive = registerRequest.IsActive,
            }, 
            registerRequest.RolesID);

            var RolesList = await authRepository.GetAllRolesByRolesID(registerRequest.RolesID);

            var Roles = RolesList.Select(r => new RoleResponse
            {
                Id = r.Id,
                RoleName = r.RoleName
            }).ToList();

            return new RegisterResponse
            {
                EmployeeID = userNew.EmployeeID,
                Username = userNew.Username,
                IsActive = userNew.IsActive,
                MustChangePassword= userNew.MustChangePassword,
                Roles = Roles
            };
        }
    }
}
