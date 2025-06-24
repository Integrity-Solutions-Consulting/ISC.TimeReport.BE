using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.application.Interfaces.Service.Leaders;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entityPerson = isc.time.report.be.domain.Entity.Persons;

namespace isc.time.report.be.application.Services.Leaders
{
    public class LeaderService : ILeaderService
    {
        private readonly ILeaderRepository _leaderRepository;
        private readonly IMapper _mapper;

        public LeaderService(ILeaderRepository leaderRepository, IMapper mapper)
        {
            _leaderRepository = leaderRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetLeaderDetailsResponse>> GetAllLeadersPaginated(PaginationParams paginationParams)
        {
            var result = await _leaderRepository.GetAllLeadersPaginatedAsync(paginationParams);
            var mapped = _mapper.Map<List<GetLeaderDetailsResponse>>(result.Items);

            return new PagedResult<GetLeaderDetailsResponse>
            {
                Items = mapped,
                TotalItems = result.TotalItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<GetLeaderDetailsResponse> GetLeaderByID(int leaderId)
        {
            var leader = await _leaderRepository.GetLeaderByIDAsync(leaderId);
            return _mapper.Map<GetLeaderDetailsResponse>(leader);
        }

        public async Task<CreateLeaderResponse> CreateLeaderWithPersonID(CreateLeaderWithPersonIDRequest request)
        {
            var leader = _mapper.Map<Leader>(request);
            var created = await _leaderRepository.CreateLeaderAsync(leader);
            created = await _leaderRepository.GetLeaderByIDAsync(leader.Id);
            return _mapper.Map<CreateLeaderResponse>(created);
        }

        public async Task<CreateLeaderResponse> CreateLeaderWithPerson(CreateLeaderWithPersonOBJRequest request)
        {
            var leader = _mapper.Map<Leader>(request);
            var created = await _leaderRepository.CreateLeaderWithPersonAsync(leader);
            return _mapper.Map<CreateLeaderResponse>(created);
        }

        public async Task<UpdateLeaderResponse> UpdateLeader(int leaderId, UpdateLeaderWithPersonIDRequest request)
        {
            var leader = await _leaderRepository.GetLeaderByIDAsync(leaderId);
            if (leader == null)
                throw new ClientFaultException("No existe el líder", 401);

            _mapper.Map(request, leader);
            var updated = await _leaderRepository.UpdateLeaderAsync(leader);
            updated = await _leaderRepository.GetLeaderByIDAsync(leader.Id);
            return _mapper.Map<UpdateLeaderResponse>(updated);
        }

        public async Task<UpdateLeaderResponse> UpdateLeaderWithPerson(int leaderId, UpdateLeaderWithPersonOBJRequest request)
        {
            var leader = await _leaderRepository.GetLeaderByIDAsync(leaderId);
            if (leader == null)
                throw new ClientFaultException("No existe el líder", 401);

            _mapper.Map(request, leader);
            var updated = await _leaderRepository.UpdateLeaderWithPersonAsync(leader);
            updated = await _leaderRepository.GetLeaderByIDAsync(updated.Id);
            return _mapper.Map<UpdateLeaderResponse>(updated);
        }

        public async Task<ActivateInactivateLeaderResponse> InactivateLeader(int leaderId)
        {
            var inactivated = await _leaderRepository.InactivateLeaderAsync(leaderId);
            return _mapper.Map<ActivateInactivateLeaderResponse>(inactivated);
        }

        public async Task<ActivateInactivateLeaderResponse> ActivateLeader(int leaderId)
        {
            var activated = await _leaderRepository.ActivateLeaderAsync(leaderId);
            return _mapper.Map<ActivateInactivateLeaderResponse>(activated);
        }
    }
}
