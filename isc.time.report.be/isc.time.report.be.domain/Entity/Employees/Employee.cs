using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Positions;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Employees
{
    public class Employee : BaseEntity
    {
        public int PersonID { get; set; }
        public Person Person { get; set; }
        public int? PositionID { get; set; }
        public Position Position { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool? ContractType { get; set; }
        public string? Department { get; set; }
        public string? CorporateEmail { get; set; }
        public decimal? Salary { get; set; }
        public ICollection<EmployeeProject> EmployeeProject { get; set; }
        public ICollection<User> User { get; set; }
    }
}
