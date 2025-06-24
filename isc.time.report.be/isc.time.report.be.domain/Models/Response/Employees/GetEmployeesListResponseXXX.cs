using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Employees
{
    public class GetEmployeesListResponseXXX
    {
        public List<GetEmployeeResponseXXX> Employees { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
