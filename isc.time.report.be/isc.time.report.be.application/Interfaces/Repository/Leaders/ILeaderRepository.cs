using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Leaders;

namespace isc.time.report.be.application.Interfaces.Repository.Leaders
{
    public interface ILeaderRepository
    {
        public Task<List<Leader>> GetLeaders();
        public Task<Leader> CreateLeader(Leader leader);
    }
}
