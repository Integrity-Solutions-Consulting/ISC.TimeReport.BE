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
        public Task<CreatePersonResponseXXX> Create(CreatePersonRequest personRequest);
        
        public Task<List<GetPersonListResponseXXX>> GetAll();
        public Task<UpdatePersonResponseXXX> Update(UpdatePersonRequest updateRequest);
    }
}
