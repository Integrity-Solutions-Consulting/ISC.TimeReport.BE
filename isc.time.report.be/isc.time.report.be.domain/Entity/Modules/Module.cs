using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Modules
{
    public class Module : BaseEntity
    {
        public string ModuleName { get; set; } = null!;
        public string? ModulePath { get; set; }
        public string? Icon { get; set; }
        public int? DisplayOrder { get; set; }
        public List<RoleModule> RoleModule { get; set; }
        public List<UserModule> UserModule { get; set; }
        public int Submodule {  get; set; }
    }
}
