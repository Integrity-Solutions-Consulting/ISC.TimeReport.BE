using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Employees;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DBContext _dbContext;
        public EmployeeRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Employee> CreateEmployee(Employee employee)
        {
            employee.CreationDate = DateTime.Now;
            employee.ModificationDate = null;
            employee.CreationIp = "0.0.0.0";
            employee.ModificationIp = "";
            employee.CreationUser = "system";
            employee.ModificationUser = "";
            employee.Status = true;
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }
        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _dbContext.Employees
                .Where(e => e.Status)
                .Include(e => e.Person)
                .Include(e => e.Position)
                .ToListAsync();
        }
    }
}
