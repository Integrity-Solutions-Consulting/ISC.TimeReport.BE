using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Customers;

namespace isc.time.report.be.application.Interfaces.Repository.Customers
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetCustomerById(int id);
        public Task<Customer> GetCustomerByName(string name);
        public Task<Customer> CreateCustomer(Customer customer);

    }
}
