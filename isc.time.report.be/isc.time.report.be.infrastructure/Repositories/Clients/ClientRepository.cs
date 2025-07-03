using isc.time.report.be.application.Interfaces.Repository.Clients;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Clients
{
    public class ClientRepository : IClientRepository
    {
        private readonly DBContext _dbContext;

        public ClientRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Client>> GetAllClientsPaginatedAsync(PaginationParams paginationParams, string? search)
        {
            var query = _dbContext.Clients
                .Include(c => c.Person)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();

                query = query.Where(c =>
                    (c.TradeName != null && c.TradeName.ToLower().Contains(normalizedSearch)) ||
                    (c.LegalName != null && c.LegalName.ToLower().Contains(normalizedSearch)) ||
                    (c.Person != null && (
                        (c.Person.FirstName != null && c.Person.FirstName.ToLower().Contains(normalizedSearch)) ||
                        (c.Person.IdentificationNumber != null && c.Person.IdentificationNumber.ToLower().Contains(normalizedSearch)) ||
                        (c.Person.Email != null && c.Person.Email.ToLower().Contains(normalizedSearch)) ||
                        (c.Person.LastName != null && c.Person.LastName.ToLower().Contains(normalizedSearch))
                    )));
            }

            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<Client> GetClientByIDAsync(int clientId)
        {
            return await _dbContext.Clients
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            client.CreationDate = DateTime.Now;
            client.ModificationDate = null;
            client.Status = true;

            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();

            return client;
        }

        public async Task<Client> CreateClientWithPersonAsync(Client client)
        {
            // Asume que client.Person viene poblado
            if (client.Person == null)
                throw new InvalidOperationException("La entidad Person no puede ser nula.");

            client.Person.CreationDate = DateTime.Now;
            client.Person.Status = true;
            client.Person.CreationUser = "SYSTEM";

            client.CreationDate = DateTime.Now;
            client.Status = true;
            client.CreationUser = "SYSTEM";

            await _dbContext.Clients.AddAsync(client); // EF también agregará la Person asociada
            await _dbContext.SaveChangesAsync();

            return client;
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            client.ModificationDate = DateTime.Now;
            _dbContext.Entry(client).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateClientWithPersonAsync(Client client)
        {
            if (client == null || client.Person == null)
                throw new InvalidOperationException("El cliente o su persona asociada no pueden ser nulos.");

            var existingClient = await _dbContext.Clients
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == client.Id);

            if (existingClient == null)
                throw new InvalidOperationException($"No existe el cliente con ID {client.Id}");


            if (client.Person.Id != existingClient.Person.Id)
                throw new InvalidOperationException("La persona ingresada, no corrsponde al ciente");

            client.Person.ModificationDate = DateTime.Now;
            client.Person.ModificationUser = "SYSTEM";

            _dbContext.Entry(existingClient.Person).CurrentValues.SetValues(client.Person);
            _dbContext.Entry(existingClient.Person).State = EntityState.Modified;

            client.ModificationDate = DateTime.Now;
            client.ModificationUser = "SYSTEM";

            _dbContext.Entry(existingClient).CurrentValues.SetValues(client);
            _dbContext.Entry(existingClient).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingClient;
        }

        public async Task<Client> InactivateClientAsync(int clientId)
        {
            var client = await _dbContext.Clients.Include(c => c.Person).FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
                throw new InvalidOperationException($"El cliente con ID {clientId} no existe.");

            client.Status = false;
            client.ModificationDate = DateTime.Now;
            _dbContext.Entry(client).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return client;
        }

        public async Task<Client> ActivateClientAsync(int clientId)
        {
            var client = await _dbContext.Clients.Include(c => c.Person).FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
                throw new InvalidOperationException($"El cliente con ID {clientId} no existe.");

            client.Status = true;
            client.ModificationDate = DateTime.Now;
            _dbContext.Entry(client).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return client;
        }
    }
}
