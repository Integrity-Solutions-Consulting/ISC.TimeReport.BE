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
        private readonly IProjectionHourProjectRepository _projectionHourProjectRepository ;
        public ProjectionHourProjectService(IProjectionHourProjectRepository projectionHourProjectRepository) { 

            _projectionHourProjectRepository = projectionHourProjectRepository ;

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
        public ProjectionHourProject MapToEntity(ProjectionHoursProjectRequest request)
        {
            return new ProjectionHourProject
            {
                Id = request.ProjectionHoursProjectId,
                ResourceTypeId = request.ResourceTypeId,
                ProjectId = request.ProjectID,
                ResourceName = request.ResourceName,
                HourlyCost = request.HourlyCost,
                ResourceQuantity = request.ResourceQuantity,
                TimeDistribution = JsonSerializer.Serialize(request.TimeDistribution), 
                TotalTime = request.TotalTime,
                ResourceCost = request.ResourceCost,
                ParticipationPercentage = request.ParticipationPercentage,
                PeriodType = request.PeriodType,
                PeriodQuantity = request.PeriodQuantity
            };
        }

        public ProjectionHoursProjectRequest MapToRequest(ProjectionHourProject entity)
        {
            return new ProjectionHoursProjectRequest
            {
                ProjectionHoursProjectId = entity.Id,
                ResourceTypeId = entity.ResourceTypeId,
                ProjectID = entity.ProjectId,
                ResourceName = entity.ResourceName,
                HourlyCost = entity.HourlyCost,
                ResourceQuantity = entity.ResourceQuantity,
                TimeDistribution = string.IsNullOrEmpty(entity.TimeDistribution)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(entity.TimeDistribution), // Deserializa el string
                TotalTime = entity.TotalTime,
                ResourceCost = entity.ResourceCost,
                ParticipationPercentage = entity.ParticipationPercentage,
                PeriodType = entity.PeriodType,
                PeriodQuantity = entity.PeriodQuantity,
            };
        }

        public async Task<ProjectionHoursProjectRequest> CreateAsync(ProjectionHoursProjectRequest request)
        {
            var entity = MapToEntity(request);

            await _projectionHourProjectRepository.CreateProjectionAsync(entity);

            return request;
        }

        public async Task<UpdateProjectionHoursProjectRequest> UpdateAsync(UpdateProjectionHoursProjectRequest request, int resourceTypeId, int projectId)
        {
            
            var entity = await _projectionHourProjectRepository.GetResourceByProjectionIdAsync(projectId, resourceTypeId);

            if (entity == null)
                throw new ClientFaultException ("Registro no encontrado", 401);

            // no tocamos period_type ni period_quantity porque son campos estaticos recuerda
            entity.ResourceTypeId = request.ResourceTypeId;
            entity.ProjectId = request.ProjectID; 
            entity.ResourceName = request.ResourceName;
            entity.HourlyCost = request.HourlyCost;
            entity.ResourceQuantity = request.ResourceQuantity;
            entity.TotalTime = request.TotalTime;
            entity.ResourceCost = request.ResourceCost;
            entity.ParticipationPercentage = request.ParticipationPercentage;

            // Serializacion
            entity.TimeDistribution = JsonSerializer.Serialize(request.TimeDistribution);

            
            await _projectionHourProjectRepository.UpdateResourceAssignedToProjectionAsync(entity, resourceTypeId, projectId);

            // Mapear 
            var response = new UpdateProjectionHoursProjectRequest
            {
                ProjectionHoursProjectId = entity.Id,
                ResourceTypeId = entity.ResourceTypeId,
                ProjectID = entity.ProjectId,
                ResourceName = entity.ResourceName,
                HourlyCost = entity.HourlyCost,
                ResourceQuantity = entity.ResourceQuantity,
                TimeDistribution = string.IsNullOrEmpty(entity.TimeDistribution)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(entity.TimeDistribution),
                TotalTime = entity.TotalTime,
                ResourceCost = entity.ResourceCost,
                ParticipationPercentage = entity.ParticipationPercentage,
                PeriodType = entity.PeriodType,
                PeriodQuantity = entity.PeriodQuantity
            };

            return response;
        }



    }
}
