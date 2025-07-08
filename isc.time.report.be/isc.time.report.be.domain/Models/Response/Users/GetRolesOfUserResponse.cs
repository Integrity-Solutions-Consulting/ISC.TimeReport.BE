using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Users
{
    public class GetRolesOfUserResponse
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public bool? IsActive { get; set; }
        public List<RoleResponse> Role { get; set; }
    }
}
