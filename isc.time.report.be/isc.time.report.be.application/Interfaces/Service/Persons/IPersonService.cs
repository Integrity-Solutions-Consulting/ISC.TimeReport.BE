using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;
using isc.time.report.be.domain.Models.Response.Persons;

namespace isc.time.report.be.application.Interfaces.Service.Persons
{
    public interface IPersonService
    {
        public Task<CreatePersonResponse> Create(CreatePersonRequest personRequest);
        
        public Task<List<GetPersonListResponse>> GetAll();
        public Task<UpdatePersonResponse> Update(UpdatePersonRequest updateRequest);
    }
}
