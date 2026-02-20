using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.application.Interfaces.Repository.Persons
{
    public interface IPersonRepository
    {
        Task<PagedResult<Person>> GetAllPersonsPaginatedAsync(PaginationParams paginationParams, string? search);
        Task<Person> GetPersonByIDAsync(int personId);
        Task<Person> CreatePersonAsync(Person person);
        Task<Person> UpdatePersonAsync(Person person);
        Task<Person> InactivatePersonAsync(int personId);
        Task<Person> ActivatePersonAsync(int personId);
    }
}
