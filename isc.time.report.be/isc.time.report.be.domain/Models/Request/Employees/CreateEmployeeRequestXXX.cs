using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Positions;

namespace isc.time.report.be.domain.Models.Response.Employees
{
    public class CreateEmployeeRequestXXX
    {
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        public int IdentificationTypeId { get; set; }
        public string IdentificationNumber { get; set; }
        public string PersonType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? PositionID { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool? ContractType { get; set; }
        public string? Department { get; set; }
        public string? CorporateEmail { get; set; }
        public decimal? Salary { get; set; }
    }
}
