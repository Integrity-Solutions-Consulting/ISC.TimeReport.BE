using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Leaders
{
    public class LeaderRepository : ILeaderRepository
    {
        private readonly DBContext _dbContext;

        public LeaderRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Leader>> GetAllLeadersPaginatedAsync(PaginationParams paginationParams)
        {
            var query = _dbContext.Leaders
                .Include(e => e.Person)
                .Include(e => e.Project)
                .AsQueryable();
            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<Leader> GetLeaderByIDAsync(int leaderId)
        {
            return await _dbContext.Leaders
                .Include(e => e.Person)
                .Include(e => e.Project)
                .FirstOrDefaultAsync(e => e.Id == leaderId);
        }

        public async Task<Leader> CreateLeaderAsync(Leader leader)
        {
            leader.CreationDate = DateTime.Now;
            leader.ModificationDate = null;
            leader.Status = true;

            await _dbContext.Leaders.AddAsync(leader);
            await _dbContext.SaveChangesAsync();

            return leader;
        }

        public async Task<Leader> CreateLeaderWithPersonAsync(Leader leader)
        {
            if (leader.Person == null)
                throw new InvalidOperationException("La entidad Person no puede ser nula.");

            leader.Person.CreationDate = DateTime.Now;
            leader.Person.Status = true;
            leader.Person.CreationUser = "SYSTEM";

            leader.CreationDate = DateTime.Now;
            leader.Status = true;
            leader.CreationUser = "SYSTEM";

            await _dbContext.Leaders.AddAsync(leader);
            await _dbContext.SaveChangesAsync();

            return leader;
        }

        public async Task<Leader> UpdateLeaderAsync(Leader leader)
        {
            leader.ModificationDate = DateTime.Now;
            _dbContext.Entry(leader).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return leader;
        }

        public async Task<Leader> UpdateLeaderWithPersonAsync(Leader leader)
        {
            if (leader == null || leader.Person == null)
                throw new InvalidOperationException("El líder o su persona asociada no pueden ser nulos.");

            var existingLeader = await _dbContext.Leaders
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == leader.Id);

            if (existingLeader == null)
                throw new InvalidOperationException($"No existe el líder con ID {leader.Id}");

            if (leader.Person.Id != existingLeader.Person.Id)
                throw new InvalidOperationException("La persona ingresada no corresponde al líder");

            leader.Person.ModificationDate = DateTime.Now;
            leader.Person.ModificationUser = "SYSTEM";

            _dbContext.Entry(existingLeader.Person).CurrentValues.SetValues(leader.Person);
            _dbContext.Entry(existingLeader.Person).State = EntityState.Modified;

            leader.ModificationDate = DateTime.Now;
            leader.ModificationUser = "SYSTEM";

            _dbContext.Entry(existingLeader).CurrentValues.SetValues(leader);
            _dbContext.Entry(existingLeader).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingLeader;
        }

        public async Task<Leader> InactivateLeaderAsync(int leaderId)
        {
            var leader = await _dbContext.Leaders.Include(e => e.Person).FirstOrDefaultAsync(e => e.Id == leaderId);
            if (leader == null)
                throw new InvalidOperationException($"El líder con ID {leaderId} no existe.");

            leader.Status = false;
            leader.ModificationDate = DateTime.Now;
            _dbContext.Entry(leader).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return leader;
        }

        public async Task<Leader> ActivateLeaderAsync(int leaderId)
        {
            var leader = await _dbContext.Leaders.Include(e => e.Person).FirstOrDefaultAsync(e => e.Id == leaderId);
            if (leader == null)
                throw new InvalidOperationException($"El líder con ID {leaderId} no existe.");

            leader.Status = true;
            leader.ModificationDate = DateTime.Now;
            _dbContext.Entry(leader).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return leader;
        }
    }
}
