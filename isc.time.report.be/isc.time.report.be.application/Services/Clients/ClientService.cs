using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Clients;
using isc.time.report.be.application.Interfaces.Service.Clients;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Clients;
using isc.time.report.be.domain.Models.Response.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<PagedResult<GetClientsDetailsResponse>> GetAllClientsPaginated(PaginationParams paginationParams)
        {
            var result = await _clientRepository.GetAllClientsPaginatedAsync(paginationParams);
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
            var created = await _clientRepository.CreateClientWithPersonAsync(client);
            created = await _clientRepository.GetClientByIDAsync(client.Id);
            return _mapper.Map<CreateClientResponse>(created);
        }

        public async Task<CreateClientResponse> CreateClientWithPerson(CreateClientWithPersonOBJRequest request)
        {
            var client = _mapper.Map<Client>(request);
            var created = await _clientRepository.CreateClientWithPersonAsync(client);
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
            var client = await _clientRepository.GetClientByIDAsync(clientId);
            if (client == null)
                throw new ClientFaultException("No existe el cliente", 401);

            var person = _mapper.Map<Person>(request.Person);
            _mapper.Map(request, client);

            var updated = await _clientRepository.UpdateClientWithPersonAsync(client, person);
            updated = await _clientRepository.GetClientByIDAsync(client.Id);
            return _mapper.Map<UpdateClientResponse>(updated);
        }

        public async Task<ActiveInactiveResponse> InactivateClient(int clientId)
        {
            var inactivated = await _clientRepository.InactivateClientAsync(clientId);
            return _mapper.Map<ActiveInactiveResponse>(inactivated);
        }

        public async Task<ActiveInactiveResponse> ActivateClient(int clientId)
        {
            var activated = await _clientRepository.ActivateClientAsync(clientId);
            return _mapper.Map<ActiveInactiveResponse>(activated);
        }
    }
}
