using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Positions;
using isc.time.report.be.domain.Entity.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Employees
{
    public class GetEmployeesPersonInfoResponse
    {
        public int Id { get; set; }
        public int PersonID { get; set; }
        public string EmployeeCode { get; set; }
        public string IdentificationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
    }
}
