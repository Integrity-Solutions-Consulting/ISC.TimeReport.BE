using isc.time.report.be.application.Interfaces.Repository.Customers;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.Customers
{
    public class ClientRepository : IClientRepository
    {
        private readonly DBContext _dbContext;
        public ClientRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Client> GetCustomerByName(string commercialname)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(/*customer => customer.CommercialName.Equals(commercialname)*/);
        }

        public async Task<Client> CreateCustomer(Client customer)
        {
            customer.CreationDate = DateTime.Now;
            customer.ModificationDate = null;
            customer.Status = true;
            await _dbContext.Clients.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Client>> GetAllCustomers()
        {
            return await _dbContext.Clients
                .Where(c => c.Status)
                .ToListAsync();
        }

        public async Task<Client> UpdateCustomer(Client customer)
        {
            customer.ModificationDate = DateTime.Now;
            _dbContext.Clients.Update(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Client?> GetCustomerById(int customerId)
        {
            return await _dbContext.Clients
                .FirstOrDefaultAsync(c=>c.Id == customerId && c.Status);
        }
    }
}
