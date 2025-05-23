﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Service.Auth;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Auth;
using isc.time.report.be.domain.Models.Response.Auth;

namespace isc.time.report.be.application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepository;
        private readonly PasswordUtils passwordUtils;
        private readonly JWTUtils jwtUtils;
        public AuthService(IAuthRepository authRepository, PasswordUtils passwordUtils, JWTUtils jwtUtils)
        {
            this.authRepository = authRepository;
            this.passwordUtils = passwordUtils;
            this.jwtUtils = jwtUtils;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var user = await authRepository.GetUserByUsername(loginRequest.Username);

            if (user == null)
            {
                throw new ClientFaultException("Usuario o contraseña incorrectos.", 401);
            }

            if (!passwordUtils.VerifyPassword(loginRequest.Password, user.Password))
            {
                throw new ClientFaultException("Usuario o contraseña incorrectos.", 401);
            }

            //await authRepository.UpdateUserLastLogin(user.Id);

            return new LoginResponse
            {
                Username = user.Username,


                //Names = user.Names,
                //Surnames = user.Surnames,
                //Email = user.Email,


                Token = jwtUtils.GenerateToken(user)
            };
        }

        public async Task<RegisterResponse> Register(RegisterRequest registerRequest)
        {
            var user = await authRepository.GetUserByUsername(registerRequest.Username);

            if (user != null)
            {
                throw new ClientFaultException("El nombre de usuario no està disponible.", 401);
            }

           

            await authRepository.CreateUser(new User
            {
                Username = registerRequest.Username,
                Password = passwordUtils.HashPassword(registerRequest.Password),
                
                //Names = registerRequest.Names,
                //Surnames = registerRequest.Surnames,
                //Email = registerRequest.Email
            
            
            });

            return new RegisterResponse();
        }
    }
}
