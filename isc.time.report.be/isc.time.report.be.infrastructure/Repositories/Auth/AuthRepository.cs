﻿using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils.Emails;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {

        private readonly DBContext _dbContext;
        private readonly EmailUtils _emailUtils;
        public AuthRepository(DBContext databaseContext, EmailUtils emailUtils)
        {
            _dbContext = databaseContext;
            _emailUtils = emailUtils;
        }
        /// <summary>
        /// ESTE SI SE USA PARA ALGO
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<User> GetUserAndRoleByUsername(string username)
        {
            var user = _dbContext.Users
                .Include(u => u.Employee)
                    .ThenInclude(e => e.Person)
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RoleModule)
                            .ThenInclude(rm => rm.Module)
                .FirstOrDefault(u => u.Username == username);

            return user;
        }
        /// <summary>
        /// SI SE USA
        /// </summary>
        /// <param name="user"></param>
        /// <param name="RolesId"></param>
        /// <returns></returns>
        public async Task<User> CreateUser(User user, List<int> roleIds, string destinatarioCorreo, string htmlCorreo)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existingUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == user.Username); 

                if (existingUser != null)
                {
                    throw new Exception($"Ya existe un usuario registrado con el Nombre {user.Username}.");
                }

                user.CreationDate = DateTime.Now;
                user.ModificationDate = null;
                user.Status = true;

                user.UserRole = new List<UserRole>();

                foreach (var roleId in roleIds)
                {
                    user.UserRole.Add(new UserRole
                    {
                        RoleID = roleId
                    });
                }

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                await _emailUtils.SendEmailAsync(destinatarioCorreo, "Credenciales de acceso", htmlCorreo);

                await transaction.CommitAsync();

                return user;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error al crear el usuario o enviar el correo.", ex);
            }
        }

        /// <summary>
        /// SI SE USA
        /// </summary>
        /// <param name="RolesId"></param>
        /// <returns></returns>
        public async Task<List<Role>> GetAllRolesByRolesID(List<int> RolesId)
        {
            if (RolesId == null || !RolesId.Any())
            {
                return new List<Role>();
            }

            var rolesList = await _dbContext.Roles
                .Where(r => RolesId.Contains(r.Id))
                .ToListAsync();

            return rolesList;
        }

        /// <summary>
        /// SI SE USA PARA ALGO
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateUserLastLoginByID(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            user.LastLogin = DateTime.Now;

            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<List<Role>> GetAllRols()
        {
            var rols = await _dbContext.Roles.ToListAsync();

            return rols;
        }

        public async Task<List<Module>> GetMenusByUsername(string username)
        {
            var menus = await _dbContext.Users
                .Where(u => u.Username == username)
                .SelectMany(u => u.UserRole) // accedemos a los roles del usuario
                .SelectMany(ur => ur.Role.RoleModule) // accedemos a los menús de esos roles
                .Select(mr => mr.Module) // obtenemos el menú
                .Distinct() // evitamos duplicados
                .ToListAsync();

            return menus;
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == name);
        }

        public async Task CreateRoleAsync(Role role)
        {
            bool exists = await _dbContext.Roles.AnyAsync(r => r.RoleName == role.RoleName);
            if (exists)
                throw new ClientFaultException($"El rol '{role.RoleName}' ya existe.");

            role.CreationDate = DateTime.Now;
            role.CreationUser = "SYSTEM";
            role.Status = true;

            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Role>> GetAllRolesWithModulesAsync()
        {
            return await _dbContext.Roles
                .Include(r => r.RoleModule.Where(rm => rm.Status == true))
                    .ThenInclude(rm => rm.Module)
                .ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _dbContext.Roles
                .Include(r => r.RoleModule)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateRoleModulesAsync(Role role, List<int> newModuleIds)
        {
            var now = DateTime.Now;
            var existing = role.RoleModule;

            var finalList = new List<RoleModule>();

            foreach (var moduleId in newModuleIds)
            {
                var existingMod = existing.FirstOrDefault(rm => rm.ModuleID == moduleId);
                if (existingMod == null)
                {
                    finalList.Add(new RoleModule
                    {
                        RoleID = role.Id,
                        ModuleID = moduleId,
                        Status = true,
                        CanView = true,
                        CreationDate = now,
                        CreationUser = "SYSTEM"
                    });
                }
                else if (!existingMod.Status)
                {
                    existingMod.Status = true;
                    existingMod.ModificationDate = now;
                    existingMod.ModificationUser = "SYSTEM";
                    finalList.Add(existingMod);
                }
                else
                {
                    finalList.Add(existingMod);
                }
            }

            foreach (var rm in existing)
            {
                if (!newModuleIds.Contains(rm.ModuleID) && rm.Status)
                {
                    rm.Status = false;
                    rm.ModificationDate = now;
                    rm.ModificationUser = "SYSTEM";
                    finalList.Add(rm);
                }
            }

            _dbContext.RoleModules.UpdateRange(finalList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserWithEmployeeAsync(string username)
        {
            return await _dbContext.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Username == username && u.Status == true);
        }
        public async Task EnviarCorreoRecuperacionPasswordAsync(string username, string html)
        {
            var user = await GetUserWithEmployeeAsync(username);

            if (user == null || user.Employee == null || string.IsNullOrWhiteSpace(user.Employee.CorporateEmail))
                throw new ClientFaultException("El usuario no tiene un correo corporativo válido registrado.", 404);

            await _emailUtils.SendEmailAsync(user.Employee.CorporateEmail, "Recuperación de contraseña", html);
        }

    }
}

