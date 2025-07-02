using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Users;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
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
        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
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
            var oee = await _dbContext.Users
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserModule)
                    .ThenInclude(um => um.Module)
                .ToListAsync();

            return oee;
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
            return await _dbContext.UserRoles
                .Where(ur => ur.UserID == userId)
                .ToListAsync();
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
            return await _dbContext.UserModules
                .Where(um => um.UserID == userId)
                .ToListAsync();
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
