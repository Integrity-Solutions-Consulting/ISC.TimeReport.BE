using AutoMapper;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Clients;
using isc.time.report.be.domain.Models.Response.Leaders;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class LeaderProfile : Profile
    {
        public LeaderProfile()
        {
            CreateMap<Leader, CreateLeaderRequest>();
            CreateMap<CreateLeaderRequest, Leader>();

            CreateMap<Leader, UpdateLeaderRequest>();
            CreateMap<UpdateLeaderRequest, Leader>();

            CreateMap<Leader, ActivateInactivateLeaderResponse>();
            CreateMap<ActiveInactiveClientResponse, Leader>();

            CreateMap<Leader, CreateLeaderResponse>();
            CreateMap<CreateLeaderResponse, Leader>();

            CreateMap<Leader, GetLeaderDetailsResponse>();
            CreateMap<GetLeaderDetailsResponse, Leader>();

            CreateMap<Leader, UpdateLeaderResponse>();
            CreateMap<UpdateLeaderResponse, Leader>();
        }
    }
}
