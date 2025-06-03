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
        public string email { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string corr { get; set; }
        public string Token { get; set; }
        public List<RoleResponse> Roles { get; set; }
        public List<GetAllUserMenusResponse> Menus { get; set; }
    }

}
