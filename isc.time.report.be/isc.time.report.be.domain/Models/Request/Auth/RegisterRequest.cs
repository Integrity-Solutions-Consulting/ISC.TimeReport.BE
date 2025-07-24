using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Auth
{
    public class RegisterRequest
    {
        public int EmployeeID { get; set; }
        public string Username { get; set; } = null!;
        public bool? IsActive { get; set; } = true;
        public List<int> RolesID { get; set; }
    }
}
