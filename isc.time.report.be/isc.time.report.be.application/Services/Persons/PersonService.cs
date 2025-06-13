using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Persons;
using entityPerson = isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Models.Response.Persons;
using isc.time.report.be.domain.Models.Response.Persons;
using isc.time.report.be.application.Interfaces.Service.Persons;

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
                //GenderId = createRequest.GenderId,
                //NationalityId = createRequest.NationalityId,
                //IdentificationTypeId = createRequest.IdentificationTypeId,
                //IdentificationNumber = createRequest.IdentificationNumber,
                //PersonType = createRequest.PersonType,
                //FirstName = createRequest.FirstName,
                //LastName = createRequest.LastName,
                //BirthDate = createRequest.BirthDate,
                //Email = createRequest.Email,
                //Phone = createRequest.Phone,
                //Address = createRequest.Address,
            };
            await PersonRepository.CreatePerson(newPerson);
            return new CreatePersonResponse();
        }

        public async Task<List<GetPersonListResponse>> GetAll()
        {
            var person = await PersonRepository.GetPersons();

            return person.Select(p => new GetPersonListResponse
            {
                //Id = p.Id,
                //GenderId = p.GenderId,
                //NationalityId = p.NationalityId,
                //IdentificationTypeId = p.IdentificationTypeId,
                //IdentificationNumber = p.IdentificationNumber,
                //PersonType = p.PersonType,
                //FirstName = p.FirstName,
                //LastName = p.LastName,
                //BirthDate= p.BirthDate,
                //Phone = p.Phone,
                //Email = p.Email,
                //Address = p.Address,
            }).ToList();
        }

        public async Task<UpdatePersonResponse> Update(UpdatePersonRequest request)
        {
            var person = await PersonRepository.GetPersonById(request.Id);

            if (person == null)
            {
                return new UpdatePersonResponse
                {
                    Success = false,
                    Message = "Persona no Encontrada"
                };
            }
            //person.Id = request.Id;
            //person.GenderId = request.GenderId; 
            //person.NationalityId = request.NationalityId;
            //person.IdentificationTypeId = request.IdentificationTypeId;
            //person.IdentificationNumber = request.IdentificationNumber;
            //person.PersonType = request.PersonType;
            //person.FirstName = request.FirstName;
            //person.LastName = request.LastName;
            //person.BirthDate = request.BirthDate;
            //person.Phone = request.Phone;
            //person.Email = request.Email;
            //person.Address = request.Address;

            await PersonRepository.UpdatePerson(person);

            return new UpdatePersonResponse
            {
                Success = true,
                Message = "Persona actualizada"
            };
        }
    }
}
