using isc.time.report.be.domain.Entity.Holidays;

namespace isc.time.report.be.application.Interfaces.Repository.Holidays
{
    public interface IHolidayRepository
    {
        Task<Holiday?> GetHolidayByIdAsync(int id);

        Task<List<Holiday>> GetAllHolidayAsync();

        Task<Holiday> CreateHolidayAsync(Holiday holiday);

        Task<Holiday> UpdateHolidayAsync(Holiday holiday);

        Task<Holiday> InactivateHolidayAsync(int id);

        Task<Holiday> ActivateHolidayAsync(int id);

    }
}
