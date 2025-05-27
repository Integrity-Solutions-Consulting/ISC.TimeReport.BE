using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Users;
using isc.time.report.be.domain.Entity.Auth;
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

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users
                .Include(u => u.UsersRols)
                    .ThenInclude(ur => ur.Rols)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _dbContext.Users
                .Include(u => u.UsersRols)
                    .ThenInclude(ur => ur.Rols)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users
                .Include(u => u.UsersRols)
                    .ThenInclude(ur => ur.Rols)
                .ToListAsync();
        }

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
                _dbContext.Users_Rols.Add(new UsersRols
                {
                    UsersId = userId,
                    RolsId = rolId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
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

    }
}
