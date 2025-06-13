using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Response.Customers;
using isc.time.report.be.domain.Models.Response.Customers;

namespace isc.time.report.be.application.Interfaces.Service.Customers
{
    public interface IClientService
    {
        public Task<CreateCustomerResponse> Create(CreateCustomerRequest customerRequest);

        public Task<List<CustomerListResponse>> GetAll();
        public Task<UpdateResponse> Update(UpdateRequest request);

    }
}
