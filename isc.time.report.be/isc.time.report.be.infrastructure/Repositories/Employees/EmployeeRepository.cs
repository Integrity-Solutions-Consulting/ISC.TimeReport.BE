using isc.time.report.be.application.Interfaces.Repository.Employees;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DBContext _dbContext;

        public EmployeeRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Employee>> GetAllEmployeesPaginatedAsync(PaginationParams paginationParams, string? search)
        {
            var query = _dbContext.Employees
                .Include(e => e.Person)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();

                query = query.Where(e =>
                    (e.EmployeeCode != null && e.EmployeeCode.ToLower().Contains(normalizedSearch)) ||
                    (e.CorporateEmail != null && e.CorporateEmail.ToLower().Contains(normalizedSearch)) ||
                    (e.Person != null && (
                        (e.Person.FirstName != null && e.Person.FirstName.ToLower().Contains(normalizedSearch)) ||
                        (e.Person.IdentificationNumber != null && e.Person.IdentificationNumber.Contains(normalizedSearch)) ||
                        (e.Person.LastName != null && e.Person.LastName.ToLower().Contains(normalizedSearch))
                    )));
            }

            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<Employee> GetEmployeeByIDAsync(int employeeId)
        {
            return await _dbContext.Employees
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            employee.CreationDate = DateTime.Now;
            employee.Status = true;
            employee.CreationUser = "SYSTEM";

            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> CreateEmployeeWithPersonAsync(Employee employee)
        {
            if (employee.Person == null)
                throw new InvalidOperationException("La entidad Person no puede ser nula.");

            employee.Person.CreationDate = DateTime.Now;
            employee.Person.Status = true;
            employee.Person.CreationUser = "SYSTEM";

            employee.CreationDate = DateTime.Now;
            employee.Status = true;
            employee.CreationUser = "SYSTEM";

            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            employee.ModificationDate = DateTime.Now;
            employee.ModificationUser = "SYSTEM";
            _dbContext.Entry(employee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeWithPersonAsync(Employee employee)
        {
            if (employee == null || employee.Person == null)
                throw new InvalidOperationException("El empleado o su persona asociada no pueden ser nulos.");

            var existingEmployee = await _dbContext.Employees
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (existingEmployee == null)
                throw new InvalidOperationException($"No existe el empleado con ID {employee.Id}");

            if (employee.Person.Id != existingEmployee.Person.Id)
                throw new InvalidOperationException("La persona ingresada no corresponde al empleado.");

            employee.Person.ModificationDate = DateTime.Now;
            employee.Person.ModificationUser = "SYSTEM";
            _dbContext.Entry(existingEmployee.Person).CurrentValues.SetValues(employee.Person);
            _dbContext.Entry(existingEmployee.Person).State = EntityState.Modified;

            employee.ModificationDate = DateTime.Now;
            employee.ModificationUser = "SYSTEM";
            _dbContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
            _dbContext.Entry(existingEmployee).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingEmployee;
        }

        public async Task<Employee> InactivateEmployeeAsync(int employeeId)
        {
            var employee = await _dbContext.Employees.Include(e => e.Person).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null)
                throw new InvalidOperationException($"El empleado con ID {employeeId} no existe.");

            employee.Status = false;
            employee.ModificationDate = DateTime.Now;
            _dbContext.Entry(employee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> ActivateEmployeeAsync(int employeeId)
        {
            var employee = await _dbContext.Employees.Include(e => e.Person).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null)
                throw new InvalidOperationException($"El empleado con ID {employeeId} no existe.");

            employee.Status = true;
            employee.ModificationDate = DateTime.Now;
            _dbContext.Entry(employee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return employee;
        }
    }
}
