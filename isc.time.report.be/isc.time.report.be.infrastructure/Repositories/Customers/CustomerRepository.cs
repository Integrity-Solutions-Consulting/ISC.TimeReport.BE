using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Customers;
using isc.time.report.be.domain.Entity.Customers;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DBContext _dbContext;
        public CustomerRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> GetCustomerByName(string commercialname)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(customer => customer.CommercialName.Equals(commercialname));
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            customer.CreatedAt = DateTime.Now;
            customer.UpdatedAt = null;
            customer.Status = true;
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _dbContext.Customers
                .Where(c => c.Status)
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int customerId)
        {
            return await _dbContext.Customers.FindAsync(customerId);
        }
    }
}
