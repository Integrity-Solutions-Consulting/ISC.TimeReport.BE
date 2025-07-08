using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Auth
{
    public class GetRolesResponse
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }
        public List<ModuleResponse> Modules { get; set; }
    }
}
