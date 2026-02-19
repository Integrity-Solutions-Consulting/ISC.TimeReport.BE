using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Request.Persons;
using isc.time.report.be.domain.Models.Response.Persons;

namespace isc.time.report.be.application.Interfaces.Service.Persons
{
    public interface IPersonService
    {
        Task<PagedResult<GetPersonResponse>> GetAllPersonsPaginated(PaginationParams paginationParams, string? search);
        Task<GetPersonResponse> GetPersonByID(int personId);
        Task<CreatePersonResponse> CreatePerson(CreatePersonRequest request);
        Task<UpdatePersonResponse> UpdatePerson(int personId, UpdatePersonRequest request);
        Task<ActiveInactivePersonResponse> InactivatePerson(int personId);
        Task<ActiveInactivePersonResponse> ActivatePerson(int personId);
    }
}
