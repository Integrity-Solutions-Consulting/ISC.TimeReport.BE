using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Employees
{
    public class EmployeeProject : BaseEntity
    {
        public int? EmployeeID { get; set; }
        public Employee? Employee { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public int? SupplierID { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public DateTime? AssignmentEndDate { get; set; }
        public string? AssignedRole { get; set; }
        public decimal? CostPerHour { get; set; }
        public decimal? AllocatedHours { get; set; }
    }
}
