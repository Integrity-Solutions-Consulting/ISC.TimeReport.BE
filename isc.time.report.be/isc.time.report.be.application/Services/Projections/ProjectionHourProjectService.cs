using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Projections;
using isc.time.report.be.application.Interfaces.Service.Projections;
using isc.time.report.be.domain.Entity.Projections;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.Projections
{
    public class ProjectionHourProjectService : IProjectionHourProjectService
    {
        private readonly IProjectionHourProjectRepository _projectionHourProjectRepository;
        private readonly IMapper _mapper;

        public ProjectionHourProjectService(IProjectionHourProjectRepository projectionHourProjectRepository, IMapper mapper) {

            _projectionHourProjectRepository = projectionHourProjectRepository;
            _mapper = mapper;

        }

        public async Task<List<ProjectionHoursProjectResponse>> GetAllProjectionByProjectId(int projectId)
        {
            var result = await _projectionHourProjectRepository.GetAllProjectionsAsync(projectId);

            if (result.Any())
            {
                return result;
            }
            else
            {
                throw new ClientFaultException("No se encontraron recursos para la proyeccion especificada.");
            }
        }


        public async Task<CreateProjectionHoursProjectResponse> CreateAsync(ProjectionHoursProjectRequest request, int projectId)
        {

            var entity = _mapper.Map<ProjectionHourProject>(request);

            entity.TimeDistribution = JsonSerializer.Serialize(request.TimeDistribution);

            await _projectionHourProjectRepository.CreateProjectionAsync(entity);


            var response = new CreateProjectionHoursProjectResponse
            {
                ResourceTypeId = entity.ResourceTypeId,
                ResourceName = entity.ResourceName,
                HourlyCost = entity.HourlyCost,
                ResourceQuantity = entity.ResourceQuantity,
                TotalTime = entity.TotalTime,
                ResourceCost = entity.ResourceCost,
                ParticipationPercentage = entity.ParticipationPercentage,
                PeriodType = entity.PeriodType,
                PeriodQuantity = entity.PeriodQuantity,
                ProjecId= entity.ProjectId,
                TimeDistribution = string.IsNullOrEmpty(entity.TimeDistribution)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(entity.TimeDistribution)


            };

            return response;
        }

        public async Task<UpdateProjectionHoursProjectResponse> UpdateAsync(UpdateProjectionHoursProjectRequest request, int resourceTypeId, int projectId)
        {

            var entity = await _projectionHourProjectRepository.GetResourceByProjectionIdAsync(projectId, resourceTypeId);

            if (entity == null)
                throw new ClientFaultException("Registro no encontrado", 401);

            // Mapeo 
            entity.ResourceTypeId = request.ResourceTypeId;
            entity.ProjectId = projectId;
            entity.ResourceName = request.ResourceName;
            entity.HourlyCost = request.HourlyCost;
            entity.ResourceQuantity = request.ResourceQuantity;
            entity.TotalTime = request.TotalTime;
            entity.ResourceCost = request.ResourceCost;
            entity.ParticipationPercentage = request.ParticipationPercentage;
           

            // Serialización 
            entity.TimeDistribution = JsonSerializer.Serialize(request.TimeDistribution);

            //Guardamos
            await _projectionHourProjectRepository.UpdateResourceAssignedToProjectionAsync(entity, resourceTypeId, projectId);

            //Mapeo actualizado
            var response = new UpdateProjectionHoursProjectResponse
            {
                ResourceTypeId = entity.ResourceTypeId,
                ProjectId = entity.ProjectId,
                ResourceName = entity.ResourceName,
                HourlyCost = entity.HourlyCost,
                ResourceQuantity = entity.ResourceQuantity,
                TimeDistribution = string.IsNullOrEmpty(entity.TimeDistribution)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(entity.TimeDistribution),
                TotalTime = entity.TotalTime,
                ResourceCost = entity.ResourceCost,
                ParticipationPercentage = entity.ParticipationPercentage,

            };

            return response;
        }

        public async Task ActivateInactiveResourceAsync (int projectId, int resourceTypeId, bool active)
        {
            var rowsAffected = await _projectionHourProjectRepository.ActiveInactiveResourceOfProjectionAsync(projectId, resourceTypeId, active);

            if (rowsAffected == 0)
            {
                throw new ServerFaultException($"Recurso {resourceTypeId} no encontrado en el projecto {projectId}");
            }
        }


    }
}
