using isc.time.report.be.application.Interfaces.Repository.Employees;
using isc.time.report.be.application.Interfaces.Service.Employees;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Employees;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Employees;
using isc.time.report.be.domain.Models.Response.Leaders;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Employees
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this._employeeService = employeeService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<SuccessResponse<CreateEmployeeResponse>>> CreateEmployee(CreateEmployeeRequest request)
        {
            var employee = await _employeeService.CreateEmployee(request);

            return Ok(new SuccessResponse<CreateEmployeeResponse>());
        }
    }
}
