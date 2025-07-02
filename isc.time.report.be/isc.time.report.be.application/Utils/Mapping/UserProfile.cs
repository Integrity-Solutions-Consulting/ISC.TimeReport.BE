using AutoMapper;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UpdatePasswordRequest>();
            CreateMap<UpdatePasswordRequest, User>();

            CreateMap<User, UpdateUserRequest>();
            CreateMap<UpdateUserRequest, User>();


            CreateMap<User, GetAllUsersResponse>()
                .ForMember(dest => dest.Role, opt =>
                    opt.MapFrom(src => src.UserRole.Select(ur => ur.Role)))
                .ForMember(dest => dest.Modules, opt =>
                    opt.MapFrom(src => src.UserModule.Select(um => um.Module)));

            CreateMap<GetAllUsersResponse, User>();

            CreateMap<Role, RoleResponse>();
            CreateMap<RoleResponse, Role>();

            CreateMap<Module, ModuleResponse>();
            CreateMap<ModuleResponse, Module>();

            CreateMap<Role, RoleResponse>();
            CreateMap<RoleResponse, Role>();

            CreateMap<User, UserResponse>();
            CreateMap<UserResponse, User>();

        }
    }
}
