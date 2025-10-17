using AutoMapper;
using isc.time.report.be.domain.Entity.ProjectionHours;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
