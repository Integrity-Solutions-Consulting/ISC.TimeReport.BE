using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Models.Response.Menus;
using isc.time.report.be.domain.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Auth
{
    public class LoginResponse
    {
        public int UserID { get; set; }
        public int EmployeeID { get; set; }
        public string TOKEN { get; set; }
        public List<RoleResponse> Roles { get; set; }
        public List<ModuleResponse> Modules { get; set; }
    }

}
