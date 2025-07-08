using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
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
        public async Task<User> CreateUser(User user, List<int> RolesId)
        {
            user.CreationDate = DateTime.Now;
            user.ModificationDate = null;
            user.Status = true;


            if (user.UserRole == null)
            {
                user.UserRole = new List<UserRole>();
            }

            foreach (var roleId in RolesId)
            {
                user.UserRole.Add(new UserRole
                {
                    UserID = user.Id,
                    RoleID = roleId
                });
            }

            await _dbContext.Users.AddAsync(user);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message); // o loguéalo
                throw;
            }


            var userRegistrado = await _dbContext.Users.FindAsync(user.Id);

            return userRegistrado;
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
            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Role>> GetAllRolesWithModulesAsync()
        {
            return await _dbContext.Roles
                .Include(r => r.RoleModule)
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

    }
}

