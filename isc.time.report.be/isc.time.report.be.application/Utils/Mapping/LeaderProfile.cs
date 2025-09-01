using AutoMapper;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Clients;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class LeaderProfile : Profile
    {
        public LeaderProfile()
        {
            CreateMap<Leader, CreateLeaderWithPersonIDRequest>();
            CreateMap<CreateLeaderWithPersonIDRequest, Leader>();

            CreateMap<Leader, CreateLeaderWithPersonOBJRequest>();
            CreateMap<CreateLeaderWithPersonOBJRequest, Leader>();

            CreateMap<Leader, UpdateLeaderWithPersonIDRequest>();
            CreateMap<UpdateLeaderWithPersonIDRequest, Leader>();

            CreateMap<Leader, UpdateLeaderWithPersonOBJRequest>();
            CreateMap<UpdateLeaderWithPersonOBJRequest, Leader>();


            CreateMap<Leader, ActivateInactivateLeaderResponse>();
            CreateMap<ActiveInactiveClientResponse, Leader>();

            CreateMap<Leader, CreateLeaderResponse>();
            CreateMap<CreateLeaderResponse, Leader>();

            CreateMap<Leader, GetLeaderDetailsResponse>();
            CreateMap<GetLeaderDetailsResponse, Leader>();

            CreateMap<Leader, UpdateLeaderResponse>();
            CreateMap<UpdateLeaderResponse, Leader>();

            CreateMap<Leader, Lider>()
                .ForMember(dest => dest.GetPersonResponse, opt => opt.MapFrom(src => src.Person));

            CreateMap<Lider, Leader>()
                .ForMember(dest => dest.Person, opt => opt.Ignore()); // se resuelve por PersonID
        }
    }
}
