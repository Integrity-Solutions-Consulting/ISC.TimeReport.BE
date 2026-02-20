using AutoMapper;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Models.Request.PermissionTypes;
using isc.time.report.be.domain.Models.Response.PermissionTypes;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class PermissionTypeProfile : Profile
    {
        public PermissionTypeProfile()
        {
            CreateMap<PermissionType, CreatePermissionTypeRequest>();
            CreateMap<CreatePermissionTypeRequest, PermissionType>();

            CreateMap<PermissionType, UpdatePermissionTypeRequest>();
            CreateMap<UpdatePermissionTypeRequest, PermissionType>();


            CreateMap<PermissionType, ActiveInactivePermissionTypeResponse>();
            CreateMap<ActiveInactivePermissionTypeResponse, PermissionType>();

            CreateMap<PermissionType, CreatePermissionTypeResponse>();
            CreateMap<CreatePermissionTypeResponse, PermissionType>();

            CreateMap<PermissionType, GetPermissionTypeResponse>();
            CreateMap<GetPermissionTypeResponse, PermissionType>();

            CreateMap<PermissionType, UpdatePermissionTypeResponse>();
            CreateMap<UpdatePermissionTypeResponse, PermissionType>();
        }
    }
}
