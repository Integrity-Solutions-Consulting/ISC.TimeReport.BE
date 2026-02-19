using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Persons;
using isc.time.report.be.application.Interfaces.Service.Persons;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Persons;
using isc.time.report.be.domain.Models.Response.Persons;

namespace isc.time.report.be.application.Services.Persons
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetPersonResponse>> GetAllPersonsPaginated(PaginationParams paginationParams, string? search)
        {
            var result = await _personRepository.GetAllPersonsPaginatedAsync(paginationParams, search);
            var mapped = _mapper.Map<List<GetPersonResponse>>(result.Items);

            return new PagedResult<GetPersonResponse>
            {
                Items = mapped,
                TotalItems = result.TotalItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<GetPersonResponse> GetPersonByID(int personId)
        {
            var person = await _personRepository.GetPersonByIDAsync(personId);
            return _mapper.Map<GetPersonResponse>(person);
        }

        public async Task<CreatePersonResponse> CreatePerson(CreatePersonRequest request)
        {
            var person = _mapper.Map<Person>(request);
            var created = await _personRepository.CreatePersonAsync(person);
            return _mapper.Map<CreatePersonResponse>(created);
        }

        public async Task<UpdatePersonResponse> UpdatePerson(int personId, UpdatePersonRequest request)
        {
            var person = await _personRepository.GetPersonByIDAsync(personId);
            if (person == null)
                throw new ClientFaultException("No existe la persona", 401);

            _mapper.Map(request, person);
            var updated = await _personRepository.UpdatePersonAsync(person);
            return _mapper.Map<UpdatePersonResponse>(updated);
        }

        public async Task<ActiveInactivePersonResponse> InactivatePerson(int personId)
        {
            var inactivated = await _personRepository.InactivatePersonAsync(personId);
            return _mapper.Map<ActiveInactivePersonResponse>(inactivated);
        }

        public async Task<ActiveInactivePersonResponse> ActivatePerson(int personId)
        {
            var activated = await _personRepository.ActivatePersonAsync(personId);
            return _mapper.Map<ActiveInactivePersonResponse>(activated);
        }
    }
}
