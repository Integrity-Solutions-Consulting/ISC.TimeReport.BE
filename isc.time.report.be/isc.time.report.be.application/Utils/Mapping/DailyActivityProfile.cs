using AutoMapper;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Models.Request.DailyActivities;
using isc.time.report.be.domain.Models.Response.DailyActivities;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class DailyActivityProfile : Profile
    {
        public DailyActivityProfile()
        {
            CreateMap<DailyActivity, AproveDailyActivityRequest>();
            CreateMap<AproveDailyActivityRequest, DailyActivity>();

            CreateMap<DailyActivity, CreateDailyActivityRequest>();
            CreateMap<CreateDailyActivityRequest, DailyActivity>();

            CreateMap<DailyActivity, UpdateDailyActivityRequest>();
            CreateMap<UpdateDailyActivityRequest, DailyActivity>();


            CreateMap<DailyActivity, ActiveInactiveDailyActivityResponse>();
            CreateMap<ActiveInactiveDailyActivityResponse, DailyActivity>();

            CreateMap<DailyActivity, CreateDailyActivityResponse>();
            CreateMap<CreateDailyActivityResponse, DailyActivity>();

            CreateMap<DailyActivity, GetDailyActivityResponse>();
            CreateMap<GetDailyActivityResponse, DailyActivity>();

            CreateMap<DailyActivity, UpdateDailyActivityResponse>();
            CreateMap<UpdateDailyActivityResponse, DailyActivity>();

        }
    }
}
