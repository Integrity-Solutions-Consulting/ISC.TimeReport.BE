using isc.time.report.be.application.Interfaces.Repository.Catalogs;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Catalogs
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly DBContext _dbContext;

        public CatalogRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ActivityType>> GetActivityTypeActivosAsync()
        {
            return await _dbContext.ActivityTypes
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<ApprovalStatus>> GetApprovalStatusActivosAsync()
        {
            return await _dbContext.ApprovalStatus
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<Department>> GetDepartmentActivosAsync()
        {
            return await _dbContext.Departments
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<Gender>> GetGenderActivosAsync()
        {
            return await _dbContext.Genders
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<IdentificationType>> GetIdentificationTypeActivosAsync()
        {
            return await _dbContext.IdentificationTypes
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<Nationality>> GetNationalityActivosAsync()
        {
            return await _dbContext.Nationality
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<PermissionType>> GetPermissionTypeActivosAsync()
        {
            return await _dbContext.PermissionTypes
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<Position>> GetPositionActivosAsync()
        {
            return await _dbContext.Positions
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<ProjectStatus>> GetProjectStatusActivosAsync()
        {
            return await _dbContext.ProjectStatus
                .Where(e => e.Status)
                .ToListAsync();
        }

        public async Task<List<ProjectType>> GetProjectTypeActivosAsync()
        {
            return await _dbContext.ProjectTypes
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<WorkMode>> GetWorkModeActivosAsync()
        {
            return await _dbContext.WorkModes
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<CompanyCatalog>> GetCompanyCatalogActivosAsync()
        {
            return await _dbContext.CompanyCatalogs
                .Where(e => e.Status)
                .ToListAsync();
        }
        public async Task<List<EmployeeCategory>> GetEmployeeCategoryActivosAsync()
        {
            return await _dbContext.EmployeeCategories
                .Where(e => e.Status)
                .ToListAsync();
        }
    }
}
