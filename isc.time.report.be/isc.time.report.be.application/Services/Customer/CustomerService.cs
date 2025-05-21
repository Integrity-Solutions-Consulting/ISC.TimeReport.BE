using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entityCustomer = isc.time.report.be.domain.Entity.Customers ;
using isc.time.report.be.application.Interfaces.Repository.Customers;
using isc.time.report.be.application.Interfaces.Service.Customers;
using isc.time.report.be.domain.Models.Response.Customers;
using System.ComponentModel.DataAnnotations;
using isc.time.report.be.domain.Models.Request.Customers;

namespace isc.time.report.be.application.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        public readonly ICustomerRepository customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }


        public async Task<CreateResponse> Create(CreateRequest createRequest)
        {
            var newCustomer = new entityCustomer.Customer
            {
                IdentificationType = createRequest.IdentificationType,
                IdentificationNumber = createRequest.IdentificationNumber,
                CommercialName = createRequest.CommercialName,
                CompanyName = createRequest.CompanyName,
                CellPhoneNumber = createRequest.CellPhoneNumber,
                Email = createRequest.Email,
            };
            await customerRepository.CreateCustomer(newCustomer);
            return new CreateResponse();
        }
    }
}
