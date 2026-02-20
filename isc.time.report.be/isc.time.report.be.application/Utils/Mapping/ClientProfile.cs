using AutoMapper;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Models.Request.Clients;
using isc.time.report.be.domain.Models.Response.Clients;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, CreateClientWithPersonIDRequest>();
            CreateMap<CreateClientWithPersonIDRequest, Client>();

            CreateMap<Client, CreateClientWithPersonOBJRequest>();
            CreateMap<CreateClientWithPersonOBJRequest, Client>();

            CreateMap<Client, UpdateClientWithPersonIDRequest>();
            CreateMap<UpdateClientWithPersonIDRequest, Client>();

            CreateMap<Client, UpdateClientWithPersonOBJRequest>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person));

            CreateMap<UpdateClientWithPersonOBJRequest, Client>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person));


            CreateMap<Client, CreateClientResponse>();
            CreateMap<CreateClientResponse, Client>();

            CreateMap<Client, UpdateClientResponse>();
            CreateMap<UpdateClientResponse, Client>();

            CreateMap<Client, GetClientsDetailsResponse>();
            CreateMap<GetClientsDetailsResponse, Client>();

            CreateMap<Client, ActiveInactiveClientResponse>();
            CreateMap<ActiveInactiveClientResponse, Client>();

            CreateMap<Client, GetClientsByEmployeeIDResponse>();
            //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //.ForMember(dest => dest.PersonID, opt => opt.MapFrom(src => src.PersonID))
            //.ForMember(dest => dest.TradeName, opt => opt.MapFrom(src => src.TradeName))
            //.ForMember(dest => dest.LegalName, opt => opt.MapFrom(src => src.LegalName));
            CreateMap<GetClientsByEmployeeIDResponse, Client>();
        }
    }
}
