using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Request.People;
using isc.time.report.be.domain.Models.Response.People;

namespace isc.time.report.be.application.Interfaces.Service.People
{
    public interface IPersonService
    {
        public Task<CreatePersonResponse> Create(CreatePersonRequest request);
    }
}
