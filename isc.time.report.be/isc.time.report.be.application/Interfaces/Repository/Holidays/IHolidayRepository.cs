using isc.time.report.be.domain.Entity.Holidays;
using isc.time.report.be.domain.Models.Response.Holidays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
