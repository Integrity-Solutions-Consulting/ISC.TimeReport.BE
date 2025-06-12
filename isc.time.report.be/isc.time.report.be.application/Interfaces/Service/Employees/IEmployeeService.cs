using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Request.Employees;
using isc.time.report.be.domain.Models.Response.Employees;

namespace isc.time.report.be.application.Interfaces.Service.Employees
{
    public interface IEmployeeService
    {
        public Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request);
    }
}
