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

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {

            if (loginRequest.email == "string" || loginRequest.Password == "string")
            {
                throw new ClientFaultException("Complete los campos faltantes.", 401);
            }

            if (string.IsNullOrWhiteSpace(loginRequest.email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                throw new ClientFaultException("Complete los campos faltantes.", 401);
            }

            var user = await authRepository.GetUserByUsername(loginRequest.email);


            if (user == null)
            {
                throw new ClientFaultException("Usuario o contraseña incorrectos.", 401);
            }

            if (!passwordUtils.VerifyPassword(loginRequest.Password, user.Password))
            {
                throw new ClientFaultException("Usuario o contraseña incorrectos.", 401);
            }

            //await authRepository.UpdateUserLastLogin(user.Id);
            var userRoles = user.UsersRols?.Select(ur => ur.Rols)
                                            .Select(r => new RoleResponse
                                            {
                                                Id = r.Id,
                                                RolName = r.RolName
                                            })
                                            .ToList() ?? new List<RoleResponse>();


            var accessibleMenuEntities = await menuRepository.GetAllMenusByUserIdDetailsAsync(user.Id);

            var accessibleMenus = accessibleMenuEntities.Select(m => new GetAllUserMenusResponse
            {
                Id = m.Id,
                NombreMenu = m.NombreMenu,
                RutaMenu = m.RutaMenu
            }).ToList();

            return new LoginResponse
            {
                email = user.email,
                Token = jwtUtils.GenerateToken(user),
                Roles = userRoles,
                Menus = accessibleMenus
            };
        }

        public async Task<RegisterResponse> Register(RegisterRequest registerRequest)
        {

            if (registerRequest.email == "string" || registerRequest.Password == "string")
            {
                throw new ClientFaultException("Complete los campos faltantes.", 401);
            }

            if (string.IsNullOrWhiteSpace(registerRequest.email) || string.IsNullOrWhiteSpace(registerRequest.Password))
            {
                throw new ClientFaultException("Complete los campos faltantes.", 401);
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(registerRequest.email))
            {
                throw new ClientFaultException("Ingrese un correo electrónico válido.", 401);
            }

            if (registerRequest.Password.Length < 8)
            {
                throw new ClientFaultException("La contraseña debe tener al menos 8 caracteres.", 401);
            }

            var user = await authRepository.GetUserByUsername(registerRequest.email);
            if (user != null)
            {
                throw new ClientFaultException("El nombre de usuario no está disponible.", 401);
            }

            await authRepository.CreateUser(new User
            {
                email = registerRequest.email,
                Password = passwordUtils.HashPassword(registerRequest.Password),   
            });

            return new RegisterResponse();
        }
    }
}
