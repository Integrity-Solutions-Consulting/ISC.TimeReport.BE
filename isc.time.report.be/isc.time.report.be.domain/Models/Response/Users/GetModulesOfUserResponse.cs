using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Users
{
    public class GetModulesOfUserResponse
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public bool? IsActive { get; set; }
        public List<ModuleResponse> Modules { get; set; }
    }
}
