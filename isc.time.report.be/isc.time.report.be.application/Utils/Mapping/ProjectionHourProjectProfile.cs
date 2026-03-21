using AutoMapper;
using isc.time.report.be.domain.Entity.Projections;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class ProjectionHourProjectProfile : Profile
    {
        public ProjectionHourProjectProfile()
        {

            CreateMap<ProjectionHourProject, ProjectionHoursProjectRequest>();
            CreateMap<ProjectionHoursProjectRequest, ProjectionHourProject>();

            CreateMap<ProjectionHourProject, UpdateProjectionHoursProjectRequest>();
            CreateMap<UpdateProjectionHoursProjectRequest, ProjectionHourProject>();


            CreateMap<ProjectionHourProject, CreateProjectionHoursProjectResponse>();
            CreateMap<CreateProjectionHoursProjectResponse, ProjectionHourProject>();

        }
    }
}
