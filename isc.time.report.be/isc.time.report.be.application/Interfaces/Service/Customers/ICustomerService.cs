using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Request.Customers;
using isc.time.report.be.domain.Models.Response.Customers;

namespace isc.time.report.be.application.Interfaces.Service.Customers
{
    public interface ICustomerService
    {
        public Task<CreateResponse> Create(CreateRequest customerRequest);

    }
}
