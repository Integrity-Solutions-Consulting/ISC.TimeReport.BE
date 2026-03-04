using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Request.Employees;
using isc.time.report.be.domain.Models.Response.Employees;

namespace isc.time.report.be.application.Interfaces.Service.Employees
{
    public interface IEmployeeService
    {
        Task<PagedResult<GetEmployeeDetailsResponse>> GetAllEmployeesPaginated(PaginationParams paginationParams, string? search);
        Task<GetEmployeeDetailsResponse> GetEmployeeByID(int employeeId);
        Task<CreateEmployeeResponse> CreateEmployeeWithPersonID(CreateEmployeeWithPersonIDRequest request);
        Task<CreateEmployeeResponse> CreateEmployeeWithPerson(CreateEmployeeWithPersonOBJRequest request);
        Task<UpdateEmployeeResponse> UpdateEmployee(int employeeId, UpdateEmployeeWithPersonIDRequest request);
        Task<UpdateEmployeeResponse> UpdateEmployeeWithPerson(int employeeId, UpdateEmployeeWithPersonOBJRequest request);
        Task<ActiveInactiveEmployeeResponse> InactivateEmployee(int employeeId);
        Task<ActiveInactiveEmployeeResponse> ActivateEmployee(int employeeId);
    }
}
