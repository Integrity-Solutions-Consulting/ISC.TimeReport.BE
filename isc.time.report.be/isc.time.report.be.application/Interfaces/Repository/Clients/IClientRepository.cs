using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Models.Response.Clients;

namespace isc.time.report.be.application.Interfaces.Repository.Clients
{
    public interface IClientRepository
    {
        public Task<Client> GetClientById(int id);
        public Task<List<Client>> GetAllClients();
        public Task<Client> CreateClient(Client client);
        public Task<Client> UpdateClient(Client client);

    }
}
