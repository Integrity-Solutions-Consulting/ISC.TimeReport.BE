using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Request.Clients;
using isc.time.report.be.domain.Models.Response.Clients;

namespace isc.time.report.be.application.Interfaces.Service.Clients
{
    public interface IClientService
    {
        Task<PagedResult<GetClientsDetailsResponse>> GetAllClientsPaginated(PaginationParams paginationParams, string? search);
        Task<PagedResult<GetClientsDetailsResponse>> GetClientsAssignedToEmployeeAsync(int employeeId, PaginationParams paginationParams, string? search);
        Task<GetClientsDetailsResponse> GetClientByID(int clientId);
        Task<CreateClientResponse> CreateClientWithPersonID(CreateClientWithPersonIDRequest request);
        Task<CreateClientResponse> CreateClientWithPerson(CreateClientWithPersonOBJRequest request);
        Task<UpdateClientResponse> UpdateClient(int clientId, UpdateClientWithPersonIDRequest request);
        Task<UpdateClientResponse> UpdateClientWithPerson(int clientId, UpdateClientWithPersonOBJRequest request);
        Task<ActiveInactiveClientResponse> InactivateClient(int clientId);
        Task<ActiveInactiveClientResponse> ActivateClient(int clientId);
        Task<List<GetClientsByEmployeeIDResponse>> GetClientsByEmployeeIdAsync(int employeeId);
        Task ValidateRepeatIdentificationNumberAsync(string identificationNumber);
    }
}
