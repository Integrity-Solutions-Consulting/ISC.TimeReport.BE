using isc.time.report.be.application.Interfaces.Repository.Holidays;
using isc.time.report.be.domain.Entity.Holidays;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.Holidays
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly DBContext _dbContext;

        public HolidayRepository(DBContext context)
        {
            _dbContext = context;
        }

        public async Task<Holiday?> GetHolidayByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ClientFaultException("El ID del Holiday no puede ser negativo");
            }
            var holiID = await _dbContext.Holidays
                .FirstOrDefaultAsync(h => h.Id == id);
            return holiID;
        }

        public async Task<List<Holiday>> GetAllHolidayAsync()
        {
            var list = await _dbContext.Holidays.ToListAsync();
            if (!list.Any())
            {
                throw new ServerFaultException("No existen Holidays");
            }
            return list;
        }
        public async Task<Holiday> CreateHolidayAsync(Holiday holiday)
        {

            holiday.CreationDate = DateTime.Now;
            holiday.CreationUser = holiday.CreationUser ?? "SYSTEM";
            holiday.CreationIp = holiday.CreationIp ?? "0.0.0.0";

            await _dbContext.Holidays.AddAsync(holiday);
            await _dbContext.SaveChangesAsync();
            return holiday;
        }

        public async Task<Holiday> UpdateHolidayAsync(Holiday holiday)
        {
            var existing = await GetHolidayByIdAsync(holiday.Id);
            if (existing == null) throw new ClientFaultException("Feriado no encontrado");

            existing.HolidayName = holiday.HolidayName;
            existing.HolidayDate = holiday.HolidayDate;
            existing.IsRecurring = holiday.IsRecurring;
            existing.HolidayType = holiday.HolidayType;
            existing.Description = holiday.Description;

            existing.ModificationDate = DateTime.Now;
            existing.ModificationUser = holiday.ModificationUser ?? "SYSTEM";
            existing.ModificationIp = holiday.ModificationIp ?? "0.0.0.0";

            _dbContext.Entry(existing).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<Holiday> InactivateHolidayAsync(int id)
        {
            var holiday = await GetHolidayByIdAsync(id);

            if (holiday == null)
                throw new ClientFaultException($"La festividad con ID {id} no existe.");

            holiday.Status = false;
            holiday.ModificationDate = DateTime.Now;
            holiday.ModificationUser = "SYSTEM";

            _dbContext.Entry(holiday).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return holiday;
        }

        public async Task<Holiday> ActivateHolidayAsync(int id)
        {
            var holiday = await GetHolidayByIdAsync(id);
            if (holiday == null)
                throw new ClientFaultException($"La festividad con ID {id} no existe.");

            holiday.Status = true;
            holiday.ModificationDate = DateTime.Now;
            holiday.ModificationUser = "SYSTEM";

            _dbContext.Entry(holiday).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return holiday;
        }
    }
}
