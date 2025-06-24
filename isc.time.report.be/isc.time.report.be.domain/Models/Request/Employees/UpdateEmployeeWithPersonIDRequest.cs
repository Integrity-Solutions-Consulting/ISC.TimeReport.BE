using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Employees
{
    public class UpdateEmployeeWithPersonIDRequest
    {
        public int PersonID { get; set; }
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
