using isc.time.report.be.application.Interfaces.Repository.Customers;
using isc.time.report.be.application.Interfaces.Service.Customers;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Customers;
using isc.time.report.be.domain.Models.Response.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using entityClient = isc.time.report.be.domain.Entity.Clients ;

namespace isc.time.report.be.application.Services.Clients
{
    public class ClientService : IClientService
    {
        public readonly IClientRepository clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }


        public async Task<CreateCustomerResponse> Create(CreateCustomerRequest createRequest)
        {
            var newClient = new entityClient.Client
            {
                //IdentificationType = createRequest.IdentificationType,
                //IdentificationNumber = createRequest.IdentificationNumber,
                //CommercialName = createRequest.CommercialName,
                //CompanyName = createRequest.CompanyName,
                //CellPhoneNumber = createRequest.CellPhoneNumber,
                //Email = createRequest.Email,
            };
            await clientRepository.CreateCustomer(newClient);
            return new CreateCustomerResponse();
        }

        public async Task<List<CustomerListResponse>> GetAll()
        {
            var customers = await clientRepository.GetAllCustomers();

            return customers.Select(c => new CustomerListResponse
            {
                //Id = c.Id.ToString(),
                //IdentificationType = c.IdentificationType,
                //IdentificationNumber = c.IdentificationNumber,
                //CommercialName = c.CommercialName,
                //CompanyName = c.CompanyName,
                //CellPhoneNumber = c.CellPhoneNumber,
                //Email = c.Email
            }).ToList();
        }
        public async Task<UpdateResponse> Update(UpdateRequest request)
        {
            var customer = await clientRepository.GetCustomerById(request.Id);

            if (customer == null) 
            {
                return new UpdateResponse
                {
                    Success = false,
                    Message = "Cliente no Encontrado"
                };
            }
            //customer.Id = request.Id;
            //customer.IdentificationType = request.IdentificationType;
            //customer.IdentificationNumber = request.IdentificationNumber;
            //customer.CommercialName = request.CommercialName;
            //customer.CompanyName = request.CompanyName;
            //customer.CellPhoneNumber = request.CellPhoneNumber;
            //customer.Email = request.Email;

            await clientRepository.UpdateCustomer(customer);

            return new UpdateResponse
            {
                Success = true,
                Message = "Cliente actualizado"
            };
        }
    }
}
