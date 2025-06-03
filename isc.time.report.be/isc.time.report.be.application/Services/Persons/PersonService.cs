using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Persons;
using entityPerson = isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Models.Response.Persons;
using isc.time.report.be.domain.Models.Request.Persons;
using isc.time.report.be.application.Interfaces.Service.Persons;
using isc.time.report.be.domain.Models.Response.Leaders;

namespace isc.time.report.be.application.Services.Person
{
    public class PersonService : IPersonService
    {
        public readonly IPersonRepository PersonRepository;

        public PersonService(IPersonRepository personRepository)
        {
            PersonRepository = personRepository;
        }
        
        public async Task<CreatePersonResponse> Create(CreatePersonRequest createRequest)
        {
            var newPerson = new entityPerson.Person
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

        public async Task<List<GetPersonListResponse>> GetAll()
        {
            var person = await PersonRepository.GetPersons();

            return person.Select(p => new GetPersonListResponse
            {
                Id = p.Id.ToString(),
                IdentificationType = p.IdentificationType,
                IdentificationNumber = p.IdentificationNumber,
                Names = p.Names,
                Surnames = p.Surnames,
                Gender = p.Gender,
                CellPhoneNumber = p.CellPhoneNumber,
                Position = p.Position,
                PersonalEmail = p.PersonalEmail,
                CorporateEmail = p.CorporateEmail,
                HomeAddress = p.HomeAddress,
            }).ToList();
        }
    }
}
