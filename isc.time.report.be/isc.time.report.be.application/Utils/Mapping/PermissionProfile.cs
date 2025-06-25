using AutoMapper;
using isc.time.report.be.domain.Entity.Permisions;
using isc.time.report.be.domain.Models.Request.Permissions;
using isc.time.report.be.domain.Models.Response.Clients;
using isc.time.report.be.domain.Models.Response.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, CreatePermissionRequest>();
            CreateMap<CreatePermissionRequest, Permission>();

            CreateMap<Permission, PermissionAproveRequest>();
            CreateMap<PermissionAproveRequest, Permission>();


            CreateMap<Permission, CreatePermissionResponse>();
            CreateMap<CreatePermissionResponse, Permission>();

            CreateMap<Permission, GetPermissionResponse>();
            CreateMap<GetPermissionResponse, Permission>();
        }
    }
}
