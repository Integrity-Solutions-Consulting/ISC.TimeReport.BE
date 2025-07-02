using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Users
{
    public class AssignModuleToUserRequest
    {
        public int UserID { get; set; }
        public List<int> ModuleIDs { get; set; } = new();
    }
}
