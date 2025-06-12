using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Models.Response.Customers;

namespace isc.time.report.be.application.Interfaces.Repository.Customers
{
    public interface IClientRepository
    {
        public Task<Client> GetCustomerById(int id);
        public Task<Client> GetCustomerByName(string name);

        public Task<List<Client>> GetAllCustomers();
        public Task<Client> CreateCustomer(Client customer);
        public Task<Client> UpdateCustomer(Client customer);

    }
}
