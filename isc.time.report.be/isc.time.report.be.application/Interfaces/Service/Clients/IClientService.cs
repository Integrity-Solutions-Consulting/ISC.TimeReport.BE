using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Request.Clients;
using isc.time.report.be.domain.Models.Response.Clients;

namespace isc.time.report.be.application.Interfaces.Service.Clients
{
    public interface IClientService
    {
        public Task<CreateClientResponse> Create(CreateClientWithPersonRequest clientRequest);

        //public Task<List<CustomerListResponse>> GetAll();
        //public Task<UpdateResponse> Update(UpdateRequest request);

    }
}
