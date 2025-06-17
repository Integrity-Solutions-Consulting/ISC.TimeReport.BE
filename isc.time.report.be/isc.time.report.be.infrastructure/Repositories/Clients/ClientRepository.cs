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

        public async Task<PagedResult<Client>> GetAllClientsPaginatedAsync(PaginationParams paginationParams)
        {
            var query = _dbContext.Clients
                .Include(c => c.Person)
                .AsQueryable();
            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<Client> GetClientByIDAsync(int clientId)
        {
            return await _dbContext.Clients
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task<Client> CreateClientWithPersonAsync(Client client)
        {
            client.CreationDate = DateTime.Now;
            client.ModificationDate = null;
            client.Status = true;

            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();

            return client;
        }

        public async Task<Client> CreateClientWithPersonAsync(Client client, Person person)
        {
            person.CreationDate = DateTime.Now;
            person.Status = true;
            person.CreationUser = "SYSTEM";
            await _dbContext.Persons.AddAsync(person);

            client.PersonID = person.Id;
            client.CreationDate = DateTime.Now;
            client.Status = true;
            client.CreationUser = "SYSTEM";

            await _dbContext.Clients.AddAsync(client);
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

        public async Task<Client> UpdateClientWithPersonAsync(Client client, Person person)
        {
            var existingPerson = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == person.Id);
            if (existingPerson == null)
                throw new InvalidOperationException("No se encontró la persona asociada al cliente.");

            _dbContext.Entry(existingPerson).CurrentValues.SetValues(person);
            _dbContext.Entry(existingPerson).State = EntityState.Modified;

            client.ModificationDate = DateTime.Now;
            _dbContext.Entry(client).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return client;
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
