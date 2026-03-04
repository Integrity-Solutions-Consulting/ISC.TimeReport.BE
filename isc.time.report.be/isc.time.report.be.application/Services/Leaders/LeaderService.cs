using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.application.Interfaces.Repository.Projects;
using isc.time.report.be.application.Interfaces.Service.Leaders;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;

namespace isc.time.report.be.application.Services.Leaders
{
    public class LeaderService : ILeaderService
    {
        private readonly ILeaderRepository _leaderRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public LeaderService(ILeaderRepository leaderRepository, IProjectRepository projectRepository, IMapper mapper)
        {
            _leaderRepository = leaderRepository;
            _projectRepository = projectRepository;
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

        public async Task<CreateLeaderResponse> CreateLeader(CreateLeaderRequest request)
        {
            var leader = _mapper.Map<Leader>(request);
            var created = await _leaderRepository.CreateLeaderAsync(leader);
            return _mapper.Map<CreateLeaderResponse>(created);
        }

        public async Task<UpdateLeaderResponse> UpdateLeader(int leaderId, UpdateLeaderRequest request)
        {
            var leader = await _leaderRepository.GetLeaderByIDAsync(leaderId);
            if (leader == null)
                throw new ClientFaultException("No existe el líder", 401);

            _mapper.Map(request, leader);
            var updated = await _leaderRepository.UpdateLeaderAsync(leader);
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

        public async Task<AssignLeaderToProjectResponse> AssignLeaderToProject(AssignLeaderToProjectRequest request)
        {
            var project = await _projectRepository.GetProjectByIDAsync(request.ProjectID);
            if (project == null)
            {
                throw new ClientFaultException("El proyecto no existe.", 404);
            }

            var leader = await _leaderRepository.GetLeaderByIDAsync(request.LeaderID);
            if (leader == null)
            {
                throw new ClientFaultException("El líder no existe.", 404);
            }

            project.LeaderID = request.LeaderID;
            await _projectRepository.UpdateProjectAsync(project);

            return new AssignLeaderToProjectResponse
            {
                ProjectID = project.Id,
                LeaderID = leader.Id,
                Message = "Líder asignado correctamente."
            };
        }

    }
}
