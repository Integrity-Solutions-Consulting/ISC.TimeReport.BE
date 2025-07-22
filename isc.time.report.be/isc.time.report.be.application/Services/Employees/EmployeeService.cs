using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Employees;
using isc.time.report.be.application.Interfaces.Repository.Persons;
using isc.time.report.be.application.Interfaces.Service.Employees;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Employees;
using isc.time.report.be.domain.Models.Response.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entityEmployee = isc.time.report.be.domain.Entity.Employees;
using entityPerson = isc.time.report.be.domain.Entity.Persons;

namespace isc.time.report.be.application.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetEmployeeDetailsResponse>> GetAllEmployeesPaginated(PaginationParams paginationParams, string? search)
        {
            var result = await _employeeRepository.GetAllEmployeesPaginatedAsync(paginationParams, search);
            var mapped = _mapper.Map<List<GetEmployeeDetailsResponse>>(result.Items);

            return new PagedResult<GetEmployeeDetailsResponse>
            {
                Items = mapped,
                TotalItems = result.TotalItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<GetEmployeeDetailsResponse> GetEmployeeByID(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeByIDAsync(employeeId);
            return _mapper.Map<GetEmployeeDetailsResponse>(employee);
        }

        public async Task<CreateEmployeeResponse> CreateEmployeeWithPersonID(CreateEmployeeWithPersonIDRequest request)
        {
            var employee = _mapper.Map<Employee>(request);
            var created = await _employeeRepository.CreateEmployeeAsync(employee);
            created = await _employeeRepository.GetEmployeeByIDAsync(employee.Id);
            return _mapper.Map<CreateEmployeeResponse>(created);
        }

        public async Task<CreateEmployeeResponse> CreateEmployeeWithPerson(CreateEmployeeWithPersonOBJRequest request)
        {
            var employee = _mapper.Map<Employee>(request);
            var created = await _employeeRepository.CreateEmployeeWithPersonForInventoryAsync(employee);
            return _mapper.Map<CreateEmployeeResponse>(created);
        }

        public async Task<UpdateEmployeeResponse> UpdateEmployee(int employeeId, UpdateEmployeeWithPersonIDRequest request)
        {
            var employee = await _employeeRepository.GetEmployeeByIDAsync(employeeId);
            if (employee == null)
                throw new ClientFaultException("No existe el empleado", 401);

            _mapper.Map(request, employee);
            var updated = await _employeeRepository.UpdateEmployeeAsync(employee);
            updated = await _employeeRepository.GetEmployeeByIDAsync(employee.Id);
            return _mapper.Map<UpdateEmployeeResponse>(updated);
        }

        public async Task<UpdateEmployeeResponse> UpdateEmployeeWithPerson(int employeeId, UpdateEmployeeWithPersonOBJRequest request)
        {
            var employee = await _employeeRepository.GetEmployeeByIDAsync(employeeId);
            if (employee == null)
                throw new ClientFaultException("No existe el empleado", 401);

            _mapper.Map(request, employee);
            var updated = await _employeeRepository.UpdateEmployeeWithPersonForInventoryAsync(employee);
            updated = await _employeeRepository.GetEmployeeByIDAsync(employee.Id);
            return _mapper.Map<UpdateEmployeeResponse>(updated);
        }

        public async Task<ActiveInactiveEmployeeResponse> InactivateEmployee(int employeeId)
        {
            var inactivated = await _employeeRepository.InactivateEmployeeForInventoryAsync(employeeId);
            return _mapper.Map<ActiveInactiveEmployeeResponse>(inactivated);
        }

        public async Task<ActiveInactiveEmployeeResponse> ActivateEmployee(int employeeId)
        {
            var activated = await _employeeRepository.ActivateEmployeeForInventoryAsync(employeeId);
            return _mapper.Map<ActiveInactiveEmployeeResponse>(activated);
        }
    }
}
