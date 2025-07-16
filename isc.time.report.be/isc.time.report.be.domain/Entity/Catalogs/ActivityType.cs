using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class ActivityType : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ColorCode { get; set; }
    }
}
