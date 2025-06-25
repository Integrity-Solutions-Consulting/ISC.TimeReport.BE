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
        public List<int> EmployeeIDs { get; set; } = new();
    }
}
