using AutoMapper;
using isc.time.report.be.domain.Entity.Holidays;
using isc.time.report.be.domain.Models.Request.Holidays;

using isc.time.report.be.domain.Models.Response.Holidays;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class HolidayProfile : Profile
    {
        public HolidayProfile()
        {
            CreateMap<Holiday, GetHolidayByIdResponse>();
            CreateMap<GetHolidayByIdResponse, Holiday>();

            CreateMap<Holiday, CreateHolidayResponse>();
            CreateMap<CreateHolidayResponse, Holiday>();

            CreateMap<Holiday, GetAllHolidayResponse>();
            CreateMap<GetAllHolidayResponse, Holiday>();

            CreateMap<Holiday, UpdateHolidayResponse>();
            CreateMap<UpdateHolidayResponse, Holiday>();

            CreateMap<Holiday, ActiveInactiveHolidayResponse>();
            CreateMap<ActiveInactiveHolidayResponse, Holiday>();


            CreateMap<Holiday, CreateHolidayRequest>();
            CreateMap<CreateHolidayRequest, Holiday>();

            CreateMap<Holiday, UpdateHolidayRequest>();
            CreateMap<UpdateHolidayRequest, Holiday>();

        }
    }
}
