using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
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
            leader.CreatedAt = DateTime.Now;
            leader.UpdatedAt = null;
            leader.Status = true;
            await dBContext.Leader.AddAsync(leader);
            Console.WriteLine(leader);
            await dBContext.SaveChangesAsync();
            return leader;
        }

        public async Task<List<Leader>> GetLeaders()
        {
            return await dBContext.Leader
                .Where(l => l.Status)
                .ToListAsync();
        }
    }
}
