using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Menu;
using isc.time.report.be.infrastructure.Database;
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
        public AuthRepository(DBContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _dbContext.Users
                .Include(u => u.UsersRols)
                    .ThenInclude(ur => ur.Rols)
                .FirstOrDefaultAsync(u => u.Username == username);

            return user;
        }

        public async Task<User> CreateUser(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = null;
            user.Status = true;
            await _dbContext.Users.AddAsync(user);

            //Aqui no faltaria un manejo de errores antes de guardar?


            await _dbContext.SaveChangesAsync();

            return user;
        }


        // NO SE USA AUN
        public async Task UpdateUserLastLogin(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            user.UpdatedAt = DateTime.Now;

            //user.LastLogin = DateTime.Now;

            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<List<Rols>> GetAllRols()
        {
            var rols = await _dbContext.Rols.ToListAsync();

            return rols;
        }

        public async Task<List<Menu>> GetMenusByUsername(string username)
        {
            var menus = await _dbContext.Users
                .Where(u => u.Username == username)
                .SelectMany(u => u.UsersRols) // accedemos a los roles del usuario
                .SelectMany(ur => ur.Rols.MenuRols) // accedemos a los menús de esos roles
                .Select(mr => mr.Menu) // obtenemos el menú
                .Distinct() // evitamos duplicados
                .ToListAsync();

            return menus;
        }
    }
}

