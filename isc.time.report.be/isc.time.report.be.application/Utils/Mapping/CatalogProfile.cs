using AutoMapper;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Models.Response.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<ActivityType, GetActivityTypeResponse>();
            CreateMap<GetActivityTypeResponse, ActivityType>();

            CreateMap<ApprovalStatus, GetApprovalStatusResponse>();
            CreateMap<GetApprovalStatusResponse, ApprovalStatus>();

            CreateMap<Department, GetDepartmentResponse>();
            CreateMap<GetDepartmentResponse, Department>();

            CreateMap<Gender, GetGenderResponse>();
            CreateMap<GetGenderResponse, Gender>();

            CreateMap<IdentificationType, GetIdentificationTypeResponse>();
            CreateMap<GetIdentificationTypeResponse, IdentificationType>();

            CreateMap<Nationality, GetNationalityResponse>();
            CreateMap<GetNationalityResponse, Nationality>();

            CreateMap<PermissionType, GetPermissionTypeResponse>();
            CreateMap<GetPermissionTypeResponse, PermissionType>();

            CreateMap<Position, GetPositionResponse>();
            CreateMap<GetPositionResponse, Position>();

            CreateMap<ProjectStatus, GetProjectStatusResponse>();
            CreateMap<GetProjectStatusResponse, ProjectStatus>();

            CreateMap<ProjectType, GetProjectTypeResponse>();
            CreateMap<GetProjectTypeResponse, ProjectType>();

            CreateMap<WorkMode, GetWorkModeResponse>();
            CreateMap<GetWorkModeResponse, WorkMode>();
        }
    }
}
