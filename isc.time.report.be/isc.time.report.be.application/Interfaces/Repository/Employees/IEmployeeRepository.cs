using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Employees;

namespace isc.time.report.be.application.Interfaces.Repository.Employees
{
    public interface IEmployeeRepository
    {
        public Task<Employee> CreateEmployee(Employee employee);
    }
}
