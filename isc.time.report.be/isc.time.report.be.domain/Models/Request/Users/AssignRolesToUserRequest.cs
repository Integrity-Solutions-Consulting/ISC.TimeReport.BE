using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Users
{
    public class AssignRolesToUserRequest
    {
        public int UserID { get; set; }
        public List<int> RolesIDs { get; set; } = new();
    }
}
