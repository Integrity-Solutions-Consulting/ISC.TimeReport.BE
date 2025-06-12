using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Permisions
{
    public class PermissionType : BaseEntity
    {
        public string TypeCode { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPaid { get; set; } = true;
        public bool RequiresApproval { get; set; } = true;
        public int? MaxDays { get; set; }
    }
}
