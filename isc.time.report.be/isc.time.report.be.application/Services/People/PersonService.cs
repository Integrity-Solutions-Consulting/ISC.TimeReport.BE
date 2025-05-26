using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.People;
using isc.time.report.be.application.Interfaces.Service.People;
using isc.time.report.be.domain.Entity.People;
using isc.time.report.be.domain.Models.Request.People;
using isc.time.report.be.domain.Models.Response.People;

namespace isc.time.report.be.application.Services.People
{
    public class PersonService : IPersonService
    {
        public readonly IPersonRepository PersonRepository;

        public PersonService(IPersonRepository personRepository)
        {
            this.PersonRepository = personRepository;
        }

        public async Task<CreatePersonResponse> Create(CreatePersonRequest createRequest)
        {
            var newPerson = new Person
            {
                IdentificationType = createRequest.IdentificationType,
                IdentificationNumber = createRequest.IdentificationNumber,
                Names = createRequest.Names,
                Surnames = createRequest.Surnames,
                Gender = createRequest.Gender,
                CellPhoneNumber = createRequest.CellPhoneNumber,
                Position = createRequest.Position,
                PersonalEmail = createRequest.PersonalEmail,
                CorporateEmail = createRequest.CorporateEmail,
                HomeAddress = createRequest.HomeAddress,
            };
            await PersonRepository.CreatePerson(newPerson);
            return new CreatePersonResponse();
        }
    }
}
