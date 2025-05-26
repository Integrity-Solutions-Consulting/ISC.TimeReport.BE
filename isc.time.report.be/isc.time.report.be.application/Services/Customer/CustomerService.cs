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
using isc.time.report.be.domain.Models.Dto;

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

        public async Task<List<CustomerListResponse>> GetAll()
        {
            var customers = await customerRepository.GetAllCustomers();

            return customers.Select(c => new CustomerListResponse
            {
                Id = c.Id.ToString(),
                IdentificationType = c.IdentificationType,
                IdentificationNumber = c.IdentificationNumber,
                CommercialName = c.CommercialName,
                CompanyName = c.CompanyName,
                CellPhoneNumber = c.CellPhoneNumber,
                Email = c.Email
            }).ToList();
        }
        public async Task<UpdateResponse> Update(UpdateRequest request)
        {
            var customer = await customerRepository.GetCustomerById(request.Id);

            if (customer == null) 
            {
                return new UpdateResponse
                {
                    Success = false,
                    Message = "Cliente no Encontrado"
                };
            }
            customer.Id = request.Id;
            customer.IdentificationType = request.IdentificationType;
            customer.IdentificationNumber = request.IdentificationNumber;
            customer.CommercialName = request.CommercialName;
            customer.CompanyName = request.CompanyName;
            customer.CellPhoneNumber = request.CellPhoneNumber;
            customer.Email = request.Email;

            await customerRepository.UpdateCustomer(customer);

            return new UpdateResponse
            {
                Success = true,
                Message = "Cliente actualizado"
            };
        }
    }
}
