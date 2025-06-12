using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.Leaders
{
    public class LeaderRepository : ILeaderRepository
    {
        private readonly DBContext dBContext;

        public LeaderRepository(DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<Leader> CreateLeader(Leader leader)
        {
            leader.CreationDate = DateTime.Now;
            leader.ModificationDate = null;
            leader.Status = true;
            leader.Person.CreationDate = DateTime.Now;
            leader.Person.ModificationDate = null;
            leader.Person.Status = true;
            await dBContext.Leaders.AddAsync(leader);
            Console.WriteLine(leader);
            await dBContext.SaveChangesAsync();
            return leader;
        }

        public async Task<List<Leader>> GetLeaders()
        {
            return await dBContext.Leaders
                .Where(l => l.Status)
                .Include(l => l.Person)
                .ToListAsync();
        }

        public async Task<Leader> UpdateLeader(Leader leader)
        {
            leader.ModificationDate = DateTime.Now;
            leader.Person.ModificationDate = DateTime.Now;
            dBContext.Leaders.Update(leader);
            await dBContext.SaveChangesAsync();
            return leader;
        }

        public async Task<Leader?> GetLeaderById(int leaderId)
        {
            return await dBContext.Leaders
                .Include(l => l.Person)
                .FirstOrDefaultAsync(l => l.Id == leaderId && l.Status);
        }
    }
}
