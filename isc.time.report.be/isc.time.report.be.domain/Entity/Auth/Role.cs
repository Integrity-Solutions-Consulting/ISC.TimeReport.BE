using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Auth
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
        public ICollection<RoleModule> RoleModule { get; set; }
    }
}
