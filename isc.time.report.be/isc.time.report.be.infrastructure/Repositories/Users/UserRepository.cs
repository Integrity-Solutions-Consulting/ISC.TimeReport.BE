using DocumentFormat.OpenXml.Spreadsheet;
using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Users;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = isc.time.report.be.domain.Entity.Auth;

namespace isc.time.report.be.infrastructure.Repositories.Users
{
    public class UserRepository : IUserRepository
    {

        private readonly DBContext _dbContext;
        public UserRepository(DBContext databaseContext)
        {
            _dbContext = databaseContext;
        }
        /// <summary>
        /// SE USAAA
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User?> GetUserById(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RoleModule)
                            .ThenInclude(rm => rm.Module)
                .Include(u => u.UserModule)
                    .ThenInclude(um => um.Module)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new ServerFaultException($"No se encontró un usuario con el ID {userId}.");
            }

            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _dbContext.Users
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// SE USAAA
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAllUsers()
        {
            var list = await _dbContext.Users
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RoleModule)
                            .ThenInclude(rm => rm.Module)
                .Include(u => u.UserModule)
                    .ThenInclude(um => um.Module)
                .ToListAsync();
            if (!list.Any())
            {
                throw new ServerFaultException("No se existe ningun usuario en la lista.");
            }
            return list;
        }


        /// <summary>
        /// SE USAAAAA
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> ResetPassword (User user)
        {
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser != null)
            {
                existingUser.PasswordHash = user.PasswordHash;
                existingUser.MustChangePassword = user.MustChangePassword;

                await _dbContext.SaveChangesAsync();
            }

            return existingUser!;
        }


        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) return false;

            //_dbContext.Users.Remove(user);
            //await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignRolesToUser(int userId, List<int> rolIds)
        {
            foreach (var rolId in rolIds)
            {
                _dbContext.UserRoles.Add(new UserRole
                {
                    UserID = userId,
                    RoleID = rolId,
                    CreationDate = DateTime.Now,
                    ModificationDate = DateTime.Now
                });
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAllRolesFromUser(int userId)
        {
            //var userRoles = _dbContext.Users_Rols.Where(ur => ur.UsersId == userId);
            //_dbContext.Users_Rols.RemoveRange(userRoles);
            //await _dbContext.SaveChangesAsync();
            return true;
        }







        public async Task<List<UserRole>> GetUserRolesAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ClientFaultException("El ID del usuario no puede ser menor o igual a 0.");
            }
            var user = await _dbContext.UserRoles
                .Where(ur => ur.UserID == userId)
                .ToListAsync();
            //if (!user.Any())
            //{
            //    throw new ServerFaultException($"No se encontraron roles para el usuario con ID {userId}.");
            //}
            return user;
        }

        public async Task SaveUserRolesAsync(List<UserRole> userRoles)
        {
            foreach (var ur in userRoles)
            {
                if (ur.Id == 0)
                    await _dbContext.UserRoles.AddAsync(ur);
                else
                    _dbContext.UserRoles.Update(ur);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserModule>> GetUserModulesAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ClientFaultException("El ID del usuario no puede ser menor o igual a 0.");
            }
            var list = await _dbContext.UserModules
                .Where(um => um.UserID == userId)
                .ToListAsync();
            if (!list.Any())
            {
                throw new ServerFaultException($"No se encontraron modulos para el usuario con ID {userId}.");
            }
            return list;
        }

        public async Task SaveUserModulesAsync(List<UserModule> userModules)
        {
            foreach (var um in userModules)
            {
                if (um.Id == 0)
                    await _dbContext.UserModules.AddAsync(um);
                else
                    _dbContext.UserModules.Update(um);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
