using isc.time.report.be.application.Interfaces.Repository.DailyActivities;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
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

        public async Task<List<DailyActivity>> GetAllAsync()
        {
            var list = await _context.DailyActivities.ToListAsync();
            if (!list.Any())
            {
                throw new ServerFaultException("No se encontraron las Daily Activities");
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

        public async Task<List<DailyActivity>> ApproveActivitiesAsync(List<int> activityIds, int employeeId, int projectId, DateTime from, DateTime to, int approverId)
        {
            var activities = await _context.DailyActivities
                .Where(a => activityIds.Contains(a.Id)
                         && a.EmployeeID == employeeId
                         && a.ProjectID == projectId
                         && a.ActivityDate >= DateOnly.FromDateTime(from)
                         && a.ActivityDate <= DateOnly.FromDateTime(to))
                .ToListAsync();

            foreach (var activity in activities)
            {
                activity.ApprovedByID = approverId;
                activity.ApprovalDate = DateTime.Now;
                activity.ModificationDate = DateTime.Now;
                activity.ModificationUser = "SYSTEM";
            }

            _context.DailyActivities.UpdateRange(activities);
            await _context.SaveChangesAsync();
            return activities;
        }
    }
}
