using isc.time.report.be.domain.Models.Request.Holidays;
using isc.time.report.be.domain.Models.Response.Holidays;

namespace isc.time.report.be.application.Interfaces.Service.Holidays
{
    public interface IHolidayService
    {
        Task<GetHolidayByIdResponse> GetHolidayByIdAsync(int id);

        Task<List<GetAllHolidayResponse>> GetAllHolidayAsync();

        Task<CreateHolidayResponse> CreateHolidayAsync(CreateHolidayRequest CreateHoliday);

        Task<UpdateHolidayResponse> UpdateHolidayAsync(UpdateHolidayRequest UpdateHoliday, int id);

        Task<ActiveInactiveHolidayResponse> ActivateHolidayAsync(int id);

        Task<ActiveInactiveHolidayResponse> InactiveHolidayAsync(int id);
    }
}
