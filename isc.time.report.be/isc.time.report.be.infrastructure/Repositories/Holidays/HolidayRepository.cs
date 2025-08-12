using isc.time.report.be.application.Interfaces.Repository.Holidays;
using isc.time.report.be.domain.Entity.Holidays;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _dbContext.Holidays
                .FirstOrDefaultAsync(h => h.Id == id /*&& h.Status*/); //Trae el primer feriado con ese ID y que esté activo
        }

        public async Task<List<Holiday>> GetAllHolidayAsync()
        {
            return await _dbContext.Holidays.ToListAsync();
        }
        public async Task<Holiday> CreateHolidayAsync (Holiday holiday)
        {
            //se establece la fecha actual, el usaurio que lo creo sino fue creado por ningun usuario sera system por defecto
            //desde que ip se creo el feriado
            holiday.CreationDate = DateTime.Now;
            // ?? devuelve un valor predeterminado si el anterior es null
            holiday.CreationUser = holiday.CreationUser ?? "SYSTEM";
            holiday.CreationIp = holiday.CreationIp ?? "0.0.0.0";

            await _dbContext.Holidays.AddAsync(holiday);
            await _dbContext.SaveChangesAsync();
            return holiday;
        }

        public async Task<Holiday> UpdateHolidayAsync (Holiday holiday)
        {
            var existing = await GetHolidayByIdAsync(holiday.Id);
            if (existing == null) throw new Exception("Feriado no encontrado");

            // Actualiza los campos editables
            existing.HolidayName = holiday.HolidayName;
            existing.HolidayDate = holiday.HolidayDate;
            existing.IsRecurring = holiday.IsRecurring;
            existing.HolidayType = holiday.HolidayType;
            existing.Description = holiday.Description;

            // Auditoría de modificación
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
                throw new ClientFaultException($"La festividad con ID {id} no existe.", 404);

            holiday.Status = false;
            //para saber quien, desde que ip y cuando lo hizo
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
                throw new ClientFaultException($"La festividad con ID {id} no existe.", 404);

            holiday.Status = true;
            holiday.ModificationDate = DateTime.Now;
            holiday.ModificationUser = "SYSTEM";

            _dbContext.Entry(holiday).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return holiday;
        }
    }
}
