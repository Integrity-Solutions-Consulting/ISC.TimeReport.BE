using AutoMapper;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Models.Request.Projects;
using isc.time.report.be.domain.Models.Response.Projects;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, CreateProjectRequest>();
            CreateMap<CreateProjectRequest, Project>();

            CreateMap<Project, UpdateProjectRequest>();

            CreateMap<UpdateProjectRequest, Project>()
                .ForMember(dest => dest.ActualEndDate,
                           opt => opt.MapFrom(src => src.ActualEndDate));



            CreateMap<Project, ActiveInactiveProjectResponse>();
            CreateMap<ActiveInactiveProjectResponse, Project>();

            CreateMap<Project, CreateProjectResponse>();
            CreateMap<CreateProjectResponse, Project>();



            CreateMap<Project, GetAllProjectsResponse>();
            CreateMap<GetAllProjectsResponse, Project>();


            CreateMap<Project, GetProjectByIDResponse>();
            CreateMap<GetProjectByIDResponse, Project>();

            CreateMap<Project, UpdateProjectResponse>();
            CreateMap<UpdateProjectResponse, Project>();

            CreateMap<Project, GetProjectsByEmployeeIDResponse>();
            CreateMap<GetProjectsByEmployeeIDResponse, Project>();
        }
    }
}
