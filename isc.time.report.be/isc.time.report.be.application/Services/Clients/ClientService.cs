using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Clients;
using isc.time.report.be.application.Interfaces.Service.Clients;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Clients;
using isc.time.report.be.domain.Models.Response.Clients;

namespace isc.time.report.be.application.Services.Clients
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetClientsDetailsResponse>> GetAllClientsPaginated(PaginationParams paginationParams, string? search)
        {
            var result = await _clientRepository.GetAllClientsPaginatedAsync(paginationParams, search);
            var mapped = _mapper.Map<List<GetClientsDetailsResponse>>(result.Items);

            return new PagedResult<GetClientsDetailsResponse>
            {
                Items = mapped,
                TotalItems = result.TotalItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<PagedResult<GetClientsDetailsResponse>> GetClientsAssignedToEmployeeAsync(int employeeId, PaginationParams paginationParams, string? search)
        {
            var result = await _clientRepository.GetClientsAssignedToEmployeeAsync(employeeId, paginationParams, search);
            var mapped = _mapper.Map<List<GetClientsDetailsResponse>>(result.Items);

            return new PagedResult<GetClientsDetailsResponse>
            {
                Items = mapped,
                TotalItems = result.TotalItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<GetClientsDetailsResponse> GetClientByID(int clientId)
        {
            var client = await _clientRepository.GetClientByIDAsync(clientId);
            return _mapper.Map<GetClientsDetailsResponse>(client);
        }

        public async Task<CreateClientResponse> CreateClientWithPersonID(CreateClientWithPersonIDRequest request)
        {
            var client = _mapper.Map<Client>(request);
            var created = await _clientRepository.CreateClientAsync(client);

            //revisar que el creado sea el mismo que el encontrado

            created = await _clientRepository.GetClientByIDAsync(client.Id);
            return _mapper.Map<CreateClientResponse>(created);
        }

        public async Task<CreateClientResponse> CreateClientWithPerson(CreateClientWithPersonOBJRequest request)
        {
            await ValidateRepeatIdentificationNumberAsync(request.Person.IdentificationNumber);

            var client = _mapper.Map<Client>(request);
            var created = await _clientRepository.CreateClientWithPersonForInventoryAsync(client);
            return _mapper.Map<CreateClientResponse>(created);
        }

        public async Task<UpdateClientResponse> UpdateClient(int clientId, UpdateClientWithPersonIDRequest request)
        {
            var client = await _clientRepository.GetClientByIDAsync(clientId);

            if (client == null)
                throw new ClientFaultException("No existe el cliente", 401);

            _mapper.Map(request, client);
            var updated = await _clientRepository.UpdateClientAsync(client);
            updated = await _clientRepository.GetClientByIDAsync(client.Id);
            return _mapper.Map<UpdateClientResponse>(updated);
        }

        public async Task<UpdateClientResponse> UpdateClientWithPerson(int clientId, UpdateClientWithPersonOBJRequest request)
        {
            if (string.IsNullOrEmpty(request.Person.IdentificationNumber))
            {
                throw new ClientFaultException("El Numero de Identificacion no puede ser nulo o estar vacio", 401);
            }

            var client = await _clientRepository.GetClientByIDAsync(clientId);
            if (client == null)
                throw new ClientFaultException("No existe el cliente", 401);

            var identification = client.Person.IdentificationNumber;

            _mapper.Map(request, client);

            var updated = await _clientRepository.UpdateClientWithPersonForInventoryAsync(identification, client);
            updated = await _clientRepository.GetClientByIDAsync(updated.Id);
            return _mapper.Map<UpdateClientResponse>(updated);
        }

        public async Task<ActiveInactiveClientResponse> InactivateClient(int clientId)
        {
            var inactivated = await _clientRepository.InactivateClientForInventoryAsync(clientId);
            return _mapper.Map<ActiveInactiveClientResponse>(inactivated);
        }

        public async Task<ActiveInactiveClientResponse> ActivateClient(int clientId)
        {
            var activated = await _clientRepository.ActivateClientForInventoryAsync(clientId);
            return _mapper.Map<ActiveInactiveClientResponse>(activated);
        }

        public async Task<List<GetClientsByEmployeeIDResponse>> GetClientsByEmployeeIdAsync(int employeeId)
        {
            var clients = await _clientRepository.GetClientsByEmployeeIdAsync(employeeId);
            return _mapper.Map<List<GetClientsByEmployeeIDResponse>>(clients);
        }

        public async Task ValidateRepeatIdentificationNumberAsync(string identificationNumber)
        {
            if (identificationNumber == null)
            {
                throw new ClientFaultException("Debe ingresar el Numero de Identificacion", 400);
            }

            var person = await _clientRepository.ValidateUNIQUEIdentificationNumberAsync(identificationNumber);

            if (person != null)
                throw new ClientFaultException(
                    $"Ya existe un Registro con numero de cedula '{identificationNumber}'.", 409
                );
        }
    }
}
