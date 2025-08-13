using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Employees
{
    public class ActiveInactiveEmployeeResponse
    {
        public int PersonID { get; set; }
        public int? PositionID { get; set; }
        public int WorkModeID { get; set; }
        public int EmployeeCategoryID { get; set; }
        public int CompanyCatalogID { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool? ContractType { get; set; }
        public int? DepartmentID { get; set; }
        public string? CorporateEmail { get; set; }
        public decimal? Salary { get; set; }
        public bool Status { get; set; }
    }
}
