using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.application.Interfaces.Repository.Employees
{
    public interface IEmployeeRepository
    {
        Task<PagedResult<Employee>> GetAllEmployeesPaginatedAsync(PaginationParams paginationParams, string? search);
        Task<Employee> GetEmployeeByIDAsync(int employeeId);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> CreateEmployeeWithPersonAsync(Employee employee);
        Task<Employee> CreateEmployeeWithPersonForInventoryAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeWithPersonAsync(Employee employee);
        Task<Employee> UpdateEmployeeWithPersonForInventoryAsync(string Identification, Employee employee);
        Task<Employee> InactivateEmployeeAsync(int employeeId);
        Task<Employee> InactivateEmployeeForInventoryAsync(int employeeId);
        Task<Employee> ActivateEmployeeAsync(int employeeId);
        Task<Employee> ActivateEmployeeForInventoryAsync(int employeeId);
        Task<Employee> GetEmployeeByCodeAsync(string employeeCode);
        Task<int?> GetProjectIdForEmployeeAsync(string employeeCode);
    }
}
