using isc.time.report.be.application.Interfaces.Repository.Catalogs;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Exceptions;
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
            var activities = await _dbContext.ActivityTypes
                .Where(e => e.Status)
                .ToListAsync();

            if (!activities.Any())
            {
                throw new ClientFaultException("No se encontraron Activity Type que esten activos");
            }
            return activities;
        }
        public async Task<List<ApprovalStatus>> GetApprovalStatusActivosAsync()
        {
            var approval =  await _dbContext.ApprovalStatus
                .Where(e => e.Status)
                .ToListAsync();
            
            if (!approval.Any())
            {
                throw new ClientFaultException("No se encontraron estados aprobados que esten activos");
            }
            return approval;

        }
        public async Task<List<Department>> GetDepartmentActivosAsync()
        {
            var department = await _dbContext.Departments
                .Where(e => e.Status)
                .ToListAsync();
            if (!department.Any())
            {
                throw new ClientFaultException("No se encontraron departamentos que esten activos");
            }
            return department;
        }
        public async Task<List<Gender>> GetGenderActivosAsync()
        {
            var gender = await _dbContext.Genders
                .Where(e => e.Status)
                .ToListAsync();

            if (!gender.Any())
            {
                throw new ClientFaultException("No se encontraron Generos que esten activos");
            }
            return gender;
        }
        public async Task<List<IdentificationType>> GetIdentificationTypeActivosAsync()
        {
            var identification = await _dbContext.IdentificationTypes
                .Where(e => e.Status)
                .ToListAsync();

            if (!identification.Any())
            {
                throw new ServerFaultException("No se encontraron Identification Type que esten activos");
            }

            return identification;
        }
        public async Task<List<Nationality>> GetNationalityActivosAsync()
        {
            var nationalities =  await _dbContext.Nationality
                .Where(e => e.Status)
                .ToListAsync();
            if (!nationalities.Any())
            {
                throw new ServerFaultException("No se encontraron nacionalidades que esten activas");
            }
            return nationalities;
        }
        public async Task<List<PermissionType>> GetPermissionTypeActivosAsync()
        {
            var permission = await _dbContext.PermissionTypes
                .Where(e => e.Status)
                .ToListAsync();
            if (!permission.Any())
            {
                throw new ServerFaultException("No se encontraron Permission Type que esten activos");
            }
            return permission;
        }
        public async Task<List<Position>> GetPositionActivosAsync()
        {
            var position = await _dbContext.Positions
                .Where(e => e.Status)
                .ToListAsync();
            if (!position.Any())
            {
                throw new ServerFaultException("No se encontraron Positions que esten activos");
            }
            return position;
        }
        public async Task<List<ProjectStatus>> GetProjectStatusActivosAsync()
        {
            var projectStatus = await _dbContext.ProjectStatus
                .Where(e => e.Status)
                .ToListAsync();
            if (!projectStatus.Any())
            {
                throw new ServerFaultException("No se encontraron Project Status que esten activos");
            }
            return projectStatus;
        }

        public async Task<List<ProjectType>> GetProjectTypeActivosAsync()
        {
            var projectType = await _dbContext.ProjectTypes
                .Where(e => e.Status)
                .ToListAsync();
            if (!projectType.Any())
            {
                throw new ServerFaultException("No se encontraron Project Type que esten activos");
            }
            return projectType;
        }
        public async Task<List<WorkMode>> GetWorkModeActivosAsync()
        {
            var workMode = await _dbContext.WorkModes
                .Where(e => e.Status)
                .ToListAsync();
            if (!workMode.Any())
            {
                throw new ServerFaultException("No se encontraron Work Mode que esten activos");
            }
            return workMode;
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
