using DocumentFormat.OpenXml.Office2010.PowerPoint;
using isc.time.report.be.application.Interfaces.Repository.DailyActivities;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.DailyActivities;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.DailyActivities
{
    public class DailyActivityRepository : IDailyActivityRepository
    {
        private readonly DBContext _context;

        public DailyActivityRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<List<DailyActivity>> GetAllAsync(int employeeId, int? month, int? year)
        {
            // Mes y año por defecto si no se envían
            int selectedMonth = month.HasValue && month.Value >= 1 && month.Value <= 12
                ? month.Value
                : DateTime.Now.Month;

            int selectedYear = year.HasValue && year.Value >= 1 && year.Value <= 9999
                ? year.Value
                : DateTime.Now.Year;

            // Primer y último día del mes
            DateOnly startOfMonth = new DateOnly(selectedYear, selectedMonth, 1);
            DateOnly startOfPreviousMonth = startOfMonth.AddMonths(-1);
            DateOnly endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var list = await _context.DailyActivities
                .Where(d => d.EmployeeID == employeeId
                            && d.ActivityDate >= startOfPreviousMonth
                            && d.ActivityDate <= endOfMonth) // incluye último día del mes
                .OrderBy(d => d.ActivityDate)
                .ToListAsync();

            if (!list.Any())
            {
                return new List<DailyActivity>();
            }

            return list;
        }




        public async Task<DailyActivity?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ClientFaultException("El ID del Daily no puede ser negativo");
            }
            var daily = await _context.DailyActivities.FindAsync(id);
            return daily;
        }

        public async Task<DailyActivity> CreateAsync(DailyActivity entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.CreationUser = "SYSTEM";
            entity.Status = true;
            await _context.DailyActivities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<DailyActivity> UpdateAsync(DailyActivity entity)
        {
            entity.ModificationDate = DateTime.Now;
            entity.ModificationUser = "SYSTEM";
            _context.DailyActivities.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("ERROR: " + ex.InnerException?.Message);
                throw;
            }
            return entity;
        }

        public async Task<DailyActivity> InactivateAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) throw new Exception("No encontrado");
            entity.Status = false;
            entity.ModificationDate = DateTime.Now;
            _context.DailyActivities.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<DailyActivity> ActivateAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) throw new Exception("No encontrado");
            entity.Status = true;
            entity.ModificationDate = DateTime.Now;
            _context.DailyActivities.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task ApproveActivitiesAsync(
                 List<int> activityIds,
                        int employeeId,
                        int projectId,
                        DateTime from,
                        DateTime to,
                        int approverId)
                        {
                            var query = _context.DailyActivities
                                .Where(a => a.ActivityDate >= DateOnly.FromDateTime(from)
                                            && a.ActivityDate <= DateOnly.FromDateTime(to));

                            if (employeeId > 0)
                                query = query.Where(a => a.EmployeeID == employeeId);

                            if (projectId > 0)
                                query = query.Where(a => a.ProjectID == projectId);

                            if (activityIds != null && activityIds.Any())
                                query = query.Where(a => activityIds.Contains(a.Id));

                            await query.ExecuteUpdateAsync(a => a
                                .SetProperty(x => x.ApprovedByID, approverId)
                                .SetProperty(x => x.ApprovalDate, DateTime.Now)
                                .SetProperty(x => x.ModificationDate, DateTime.Now)
                                .SetProperty(x => x.ModificationUser, "SYSTEM")
                            );
        }

        public async Task<List<DailyActivity>> GetActivitiesForApprovalAsync(
            List<int> activityIds,
                    int employeeId,
                    int projectId,
                    DateTime from,
                    DateTime to)
                    {
                        var query = _context.DailyActivities
                            .Where(a => a.ActivityDate >= DateOnly.FromDateTime(from)
                                        && a.ActivityDate <= DateOnly.FromDateTime(to));

                        if (employeeId > 0)
                            query = query.Where(a => a.EmployeeID == employeeId);

                        if (projectId > 0)
                            query = query.Where(a => a.ProjectID == projectId);


                        return await query.ToListAsync();
                    }




        public async Task AddRangeAsync(List<DailyActivity> activities)
        {
            // Solo agregamos las entidades, no validaciones ni asignaciones de negocio
            _context.DailyActivities.AddRange(activities);
            await _context.SaveChangesAsync();
        }

        public async Task<string?> GetActivityTypeNameByIdAsync(int activityTypeId)
        {
            var activityType = await _context.ActivityTypes
                .Where(a => a.Id == activityTypeId && a.Status)
                .Select(a => a.Name)
                .FirstOrDefaultAsync();

            return activityType; // Devuelve null si no existe
        }

        public async Task<bool> ExistsApprovedActivitiesAsync(int employeeId, int month, int year)
        {
            return await _context.DailyActivities
                .AnyAsync(a => a.EmployeeID == employeeId
                               && a.ActivityDate.Month == month
                               && a.ActivityDate.Year == year
                               && a.ApprovedByID != null);
        }



    }
}
