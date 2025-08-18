using isc.time.report.be.application.Interfaces.Repository.Employees;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Dto.InventorysApis;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysEmployee;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Repositories.InventorysApis;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DBContext _dbContext;
        private readonly InventoryApiRepository inventoryApiRepository;

        public EmployeeRepository(DBContext dbContext, InventoryApiRepository inventoryApiRepository)
        {
            _dbContext = dbContext;
            this.inventoryApiRepository = inventoryApiRepository;
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
            if(employeeId <= 0)
            {
                throw new ClientFaultException("La ID del empleado no puede ser negativa");
            }

            var employee = await _dbContext.Employees
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
            return employee;
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
                throw new ClientFaultException("La entidad Person no puede ser nula.");

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

        //public async Task<Employee> CreateEmployeeWithPersonForInventoryAsync(Employee employee)
        //{

        //    if (employee.Person == null)
        //        throw new ClientFaultException("La entidad Person no puede ser nula.");

        //    var existingEmployee = await _dbContext.Employees
        //        .FirstOrDefaultAsync(p => p.Person.IdentificationNumber == employee.Person.IdentificationNumber);

        //    if (existingEmployee != null)
        //    {
        //        throw new ClientFaultException($"Ya existe un empleado con ese Numero de Identificacion '{employee.Person.IdentificationNumber}'.");
        //    }

        //    var invEmployee = new InventoryCreateEmployeeRequest
        //    {
        //        idIdentificationType = employee.Person?.IdentificationTypeId ?? 0,
        //        idGender = employee.Person?.GenderID ?? 0,
        //        idPosition = employee.PositionID ?? 0,
        //        idWorkMode = employee.WorkModeID,
        //        idNationality = employee.Person?.NationalityId ?? 0,
        //        firstName = employee.Person?.FirstName,
        //        lastName = employee.Person?.LastName,
        //        identification = employee.Person?.IdentificationNumber,
        //        phone = employee.Person?.Phone,
        //        email = employee.CorporateEmail,
        //        address = employee.Person?.Address,
        //        contractDate = DateOnly.FromDateTime(employee.HireDate ?? DateTime.MinValue),
        //        contractEndDate = employee.TerminationDate.HasValue
        //            ? DateOnly.FromDateTime(employee.TerminationDate.Value)
        //            : null
        //    };

        //    using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            employee.Person.CreationDate = DateTime.Now;
        //            employee.Person.Status = true;
        //            employee.Person.CreationUser = "SYSTEM";

        //            employee.CreationDate = DateTime.Now;
        //            employee.Status = true;
        //            employee.CreationUser = "SYSTEM";

        //            await _dbContext.Employees.AddAsync(employee);


        //            var invEmpInsrt = await inventoryApiRepository.CreateEmployeeInventoryAsync(invEmployee);

        //            if (invEmpInsrt == null)
        //                throw new ClientFaultException("No se pudo crear el empleado en el sistema de inventario.");

        //            await _dbContext.SaveChangesAsync();

        //            await transaction.CommitAsync();

        //            return employee;
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();

        //            // Esto te mostrará el mensaje real de la base de datos
        //            var inner = ex.InnerException?.Message ?? ex.Message;
        //            throw new Exception($"Error al guardar: {inner}", ex);
        //        }

        //    }
        //}

        public async Task<Employee> CreateEmployeeWithPersonForInventoryAsync(Employee employee)
        {
            if (employee.Person == null)
                throw new ClientFaultException("La entidad Person no puede ser nula.");

            // Verificar duplicados incluyendo la relación Person
            var existingEmployee = await _dbContext.Employees
                .Include(e => e.Person)
                .FirstOrDefaultAsync(p => p.Person.IdentificationNumber == employee.Person.IdentificationNumber);

            if (existingEmployee != null)
                throw new ClientFaultException(
                    $"Ya existe un empleado con ese Número de Identificación '{employee.Person.IdentificationNumber}'."
                );

            var invEmployee = new InventoryCreateEmployeeRequest
            {
                idIdentificationType = employee.Person?.IdentificationTypeId ?? 0,
                idGender = employee.Person?.GenderID ?? 0,
                idPosition = employee.PositionID ?? 0,
                idWorkMode = employee.WorkModeID,
                idNationality = employee.Person?.NationalityId ?? 0,
                firstName = employee.Person?.FirstName,
                lastName = employee.Person?.LastName,
                identification = employee.Person?.IdentificationNumber,
                phone = employee.Person.Phone,
                email = employee.CorporateEmail,
                address = employee.Person.Address,
                contractDate = employee.HireDate.HasValue
                    ? DateOnly.FromDateTime(employee.HireDate.Value)
                    : throw new ClientFaultException("La fecha de contratación es obligatoria."),
                contractEndDate = employee.TerminationDate.HasValue
                    ? DateOnly.FromDateTime(employee.TerminationDate.Value)
                    : null
            };

            // Transacción local para asegurar consistencia
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                employee.Person.CreationDate = DateTime.Now;
                employee.Person.Status = true;
                employee.Person.CreationUser = "SYSTEM";

                employee.CreationDate = DateTime.Now;
                employee.Status = true;
                employee.CreationUser = "SYSTEM";

                await _dbContext.Employees.AddAsync(employee);
                await _dbContext.SaveChangesAsync(); // Inserción local

                // Llamada a API externa
                var invEmpInsert = await inventoryApiRepository.CreateEmployeeInventoryAsync(invEmployee);
                if (invEmpInsert == null)
                    throw new ClientFaultException("No se pudo crear el empleado en el sistema de inventario.");

                await transaction.CommitAsync();
                return employee;
            }
            catch
            {
                await transaction.RollbackAsync(); // Revierte la inserción local
                throw;
            }
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
                throw new ServerFaultException("El empleado o su persona asociada no pueden ser nulos.");

            var existingEmployee = await _dbContext.Employees
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (existingEmployee == null)
                throw new ServerFaultException($"No existe el empleado con ID {employee.Id}");

            if (employee.Person.Id != existingEmployee.Person.Id)
                throw new ServerFaultException("La persona ingresada no corresponde al empleado.");

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

        public async Task<Employee> UpdateEmployeeWithPersonForInventoryAsync(string Identification, Employee employee)
        {
            if (employee == null || employee.Person == null)
                throw new ServerFaultException("El empleado o su persona asociada no pueden ser nulos.");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existingEmployee = await _dbContext.Employees
                    .Include(e => e.Person)
                    .FirstOrDefaultAsync(e => e.Id == employee.Id);

                if (existingEmployee == null)
                    throw new ServerFaultException($"No existe el empleado con ID {employee.Id}");

                if (employee.Person.Id != existingEmployee.Person.Id)
                    throw new ServerFaultException("La persona ingresada no corresponde al empleado.");

                existingEmployee.Person.IdentificationNumber = employee.Person.IdentificationNumber;
                existingEmployee.Person.PersonType = employee.Person.PersonType;
                existingEmployee.Person.FirstName = employee.Person.FirstName;
                existingEmployee.Person.LastName = employee.Person.LastName;
                existingEmployee.Person.BirthDate = employee.Person.BirthDate;
                existingEmployee.Person.Email = employee.Person.Email;
                existingEmployee.Person.Phone = employee.Person.Phone;
                existingEmployee.Person.Address = employee.Person.Address;
                existingEmployee.Person.GenderID = employee.Person.GenderID;
                existingEmployee.Person.NationalityId = employee.Person.NationalityId;
                existingEmployee.Person.IdentificationTypeId = employee.Person.IdentificationTypeId;
                existingEmployee.Person.ModificationDate = DateTime.Now;
                existingEmployee.Person.ModificationUser = "SYSTEM";

                existingEmployee.PositionID = employee.PositionID;
                existingEmployee.WorkModeID = employee.WorkModeID;
                existingEmployee.EmployeeCategoryID = employee.EmployeeCategoryID;
                existingEmployee.CompanyCatalogID = employee.CompanyCatalogID;
                existingEmployee.EmployeeCode = employee.EmployeeCode;
                existingEmployee.HireDate = employee.HireDate;
                existingEmployee.TerminationDate = employee.TerminationDate;
                existingEmployee.ContractType = employee.ContractType;
                existingEmployee.CorporateEmail = employee.CorporateEmail;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.ModificationDate = DateTime.Now;
                existingEmployee.ModificationUser = "SYSTEM";

                _dbContext.Entry(existingEmployee.Person).State = EntityState.Modified;
                _dbContext.Entry(existingEmployee).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();

                var inventoryUpdateRequest = new InventoryUpdateEmployeeRequest
                {
                    idIdentificationType = employee.Person.IdentificationTypeId ?? 0,
                    idGender = employee.Person.GenderID ?? 0,
                    idPosition = employee.PositionID ?? 0,
                    idWorkMode = employee.WorkModeID,
                    idNationality = employee.Person.NationalityId ?? 0,
                    firstName = employee.Person.FirstName,
                    lastName = employee.Person.LastName,
                    identification = employee.Person.IdentificationNumber,
                    phone = employee.Person.Phone,
                    email = employee.CorporateEmail,
                    address = employee.Person.Address,
                    contractDate = employee.HireDate.HasValue
                        ? DateOnly.FromDateTime(employee.HireDate.Value)
                        : DateOnly.MinValue,
                    contractEndDate = employee.TerminationDate.HasValue
                        ? DateOnly.FromDateTime(employee.TerminationDate.Value)
                        : null
                };

                var updated = await inventoryApiRepository.UpdateEmployeeInventoryAsync(inventoryUpdateRequest, Identification);
                if (!updated)
                    throw new ServerFaultException("No se pudo actualizar el empleado en el sistema de inventario.");

                await transaction.CommitAsync();
                return existingEmployee;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Conflicto de concurrencia al intentar guardar. Refresca los datos e intenta nuevamente.", ex);
            }
            catch (ValidationException ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Error de validación en los datos enviados.", ex);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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

        public async Task<Employee> InactivateEmployeeForInventoryAsync(int employeeId)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var employee = await _dbContext.Employees
                    .Include(e => e.Person)
                    .FirstOrDefaultAsync(e => e.Id == employeeId);

                if (employee == null)
                    throw new ClientFaultException($"El empleado con ID {employeeId} no existe.", 404);

                employee.Status = false;
                employee.ModificationDate = DateTime.Now;
                employee.ModificationUser = "SYSTEM";

                _dbContext.Entry(employee).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                var success = await inventoryApiRepository.InactivateStatusEmployeeInventoryAsync(employee.Person.IdentificationNumber);
                if (!success)
                    throw new ServerFaultException("No se pudo desactivar el empleado en el sistema de inventario.");

                await transaction.CommitAsync();
                return employee;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Employee> ActivateEmployeeAsync(int employeeId)
        {
            var employee = await _dbContext.Employees.Include(e => e.Person).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null)
                throw new ServerFaultException($"El empleado con ID {employeeId} no existe.");

            employee.Status = true;
            employee.ModificationDate = DateTime.Now;
            _dbContext.Entry(employee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> ActivateEmployeeForInventoryAsync(int employeeId)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var employee = await _dbContext.Employees
                    .Include(e => e.Person)
                    .FirstOrDefaultAsync(e => e.Id == employeeId);

                if (employee == null)
                    throw new ClientFaultException($"El empleado con ID {employeeId} no existe.", 404);

                employee.Status = true;
                employee.ModificationDate = DateTime.Now;
                employee.ModificationUser = "SYSTEM";

                _dbContext.Entry(employee).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                var success = await inventoryApiRepository.ActivateStatusEmployeeInventoryAsync(employee.Person.IdentificationNumber);
                if (!success)
                    throw new ServerFaultException("No se pudo activar el empleado en el sistema de inventario.");

                await transaction.CommitAsync();
                return employee;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
