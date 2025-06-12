using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Modules
{
    public class RoleModule : BaseEntity
    {
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public int ModuleID { get; set; }
        public Module Module { get; set; }
        public bool CanView { get; set; } = true;
        public bool CanCreate { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
    }
}
