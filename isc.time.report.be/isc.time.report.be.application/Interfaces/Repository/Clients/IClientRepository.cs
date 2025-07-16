using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.Clients
{
    public interface IClientRepository
    {
        Task<PagedResult<Client>> GetAllClientsPaginatedAsync(PaginationParams paginationParams, string? search);
        Task<Client> GetClientByIDAsync(int clientId);
        Task<Client> CreateClientAsync(Client client);
        Task<Client> CreateClientWithPersonAsync(Client client);
        Task<Client> UpdateClientAsync(Client client);
        Task<Client> UpdateClientWithPersonAsync(Client client);
        Task<Client> InactivateClientAsync(int clientId);
        Task<Client> ActivateClientAsync(int clientId);
        Task<List<Client>> GetClientsByEmployeeIdAsync(int employeeId);

    }
}
