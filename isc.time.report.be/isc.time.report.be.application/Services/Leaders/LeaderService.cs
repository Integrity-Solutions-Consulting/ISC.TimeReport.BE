using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.application.Interfaces.Service.Leaders;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;
using isc.time.report.be.domain.Models.Response.Projects;
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

        public async Task<PagedResult<GetLeaderDetailsResponse>> GetAllLeadersPaginated(PaginationParams paginationParams, string? search)
        {
            var result = await _leaderRepository.GetAllLeadersPaginatedAsync(paginationParams, search);
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

            //ESTO HAY QUE CAMBIARLO CUANDO EL FRONTEND AREGLE LO DE NATIONALITY
            if (request.Person.NationalityId == null || request.Person.NationalityId == 0)
            {
                request.Person.NationalityId = 5;
            }

            if (request.LeadershipType == null)
            {
                throw new ClientFaultException("El tipo de liderazgo no puede ser nulo", 400);
            }

            if (request.LeadershipType == false)
            {
                var random = new Random();
                var numerosAleatorios = random.Next(100_000_000, 1_000_000_000);
                request.Person.IdentificationNumber = $"L{numerosAleatorios}";
            }

            if (request.LeadershipType == true && string.IsNullOrEmpty(request.Person.IdentificationNumber))
            {
                throw new ClientFaultException("El numero de cedula de un lider interno no puede ser null o estar vacio", 400);
            }

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
            if (request.LeadershipType == false)
            {
                request.Person.IdentificationNumber = leader.Person.IdentificationNumber;
            }

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


        public async Task<List<GetAllLeaderProjectByPersonIdResponse>> GetAllLeadersRegisterGrouped()
        {
            var leaders = await _leaderRepository.GetAllLeaderProjectByPersonIdAsync();

            var response = leaders
                .GroupBy(l => l.PersonID)
                .Select(g =>
                {
                    var firstLeader = g.First();
                    return new GetAllLeaderProjectByPersonIdResponse
                    {
                        Person = firstLeader.Person != null ? new GetPersonResponse
                        {
                            Id = firstLeader.Person.Id,
                            IdentificationNumber = firstLeader.Person.IdentificationNumber,
                            GenderId = (int)firstLeader.Person.GenderID,
                            NationalityId = (int)firstLeader.Person.NationalityId,
                            IdentificationTypeId = (int)firstLeader.Person.IdentificationTypeId,
                            FirstName = firstLeader.Person.FirstName,
                            LastName = firstLeader.Person.LastName,
                            Email = firstLeader.Person.Email,
                            Phone = firstLeader.Person.Phone,
                            Address = firstLeader.Person.Address,
                            Status = firstLeader.Person.Status
                        } : null,
                        LeaderMiddle = g.Select(l => new LeaderData
                        {
                            Id = l.ProjectID,
                            Responsibility = l.Responsibilities,
                            LeadershipType = l.LeadershipType,
                            StartDate = l.StartDate,
                            EndDate = l.EndDate,
                            Status = l.Status
                        }).ToList()
                    };
                })
                .ToList();

            return response;
        }

        public async Task AssignPersonToProject(AssignPersonToProjectRequest request)
        {
            var existing = await _leaderRepository.GetAllLeaderProjectByPersonIdAsync();
            var now = DateTime.UtcNow;
            var finalList = new List<Leader>();

            foreach (var dto in request.PersonProjectMiddle)
            {
                var match = existing.FirstOrDefault(l => l.ProjectID == dto.ProjectID);
            }



        }

        public async Task<GetAllLeaderProjectByPersonIdResponse?> GetLeadershipByPersonId(int personId)
        {
            var leadership = await _leaderRepository.GetLeadershipByPersonId(personId);

            if (leadership == null)
                return null;

                    var response = new GetAllLeaderProjectByPersonIdResponse
                    {
                        Person = leadership != null ? new GetPersonResponse
                        {
                            Id = leadership.Id,
                            IdentificationNumber = leadership.IdentificationNumber,
                            GenderId = (int)leadership.GenderID,
                            NationalityId = (int)leadership.NationalityId,
                            IdentificationTypeId = (int)leadership.IdentificationTypeId,
                            FirstName = leadership.FirstName,
                            LastName = leadership.LastName,
                            Email = leadership.Email,
                            Phone = leadership.Phone,
                            Address = leadership.Address,
                            Status = leadership.Status
                        } : null,
                        LeaderMiddle = leadership.Leader.Select(l => new LeaderData
                        {
                            Id = l.ProjectID,
                            Responsibility = l.Responsibilities,
                            LeadershipType = l.LeadershipType,
                            StartDate = l.StartDate,
                            EndDate = l.EndDate,
                            Status = l.Status,
                            Projectos = l.Project != null ? new GetAllProjectsResponse
                            {
                                Id = l.Project.Id,
                                Name = l.Project.Name
                            } : null
                        }).ToList()
                    };

            return response;
        }

    }
}
