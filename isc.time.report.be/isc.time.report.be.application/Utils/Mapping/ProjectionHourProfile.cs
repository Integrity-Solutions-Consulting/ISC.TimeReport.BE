using AutoMapper;
using isc.time.report.be.domain.Entity.ProjectionHours;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class ProjectionHourProfile : Profile
    {
        public ProjectionHourProfile()
        {
            CreateMap<ProjectionHour, CreateProjectionWithoutProjectRequest>();
            CreateMap<CreateProjectionWithoutProjectRequest, ProjectionHour>();

            CreateMap<ProjectionHour, UpdateProjectionWithoutProjectRequest>();
            CreateMap<UpdateProjectionWithoutProjectRequest, ProjectionHour>();


            CreateMap<ProjectionHour, CreateProjectionWithoutProjectResponse>();
            CreateMap<CreateProjectionWithoutProjectResponse, ProjectionHour>();
        }
    }
}
