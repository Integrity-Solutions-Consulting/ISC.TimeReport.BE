using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Projects
{
    public class AssignEmployeesToProjectRequest
    {
        public int ProjectID { get; set; }
        public List<EmployeeProjectMiddleRequest> EmployeeProjectMiddle { get; set; } = new();
    }

    public class EmployeeProjectMiddleRequest
    {
        public int? EmployeeId { get; set; }
        public int? SupplierID { get; set; }
        public string AssignedRole { get; set; }
        public decimal CostPerHour { get; set; }
        public decimal AllocatedHours { get; set; }
    }
}
