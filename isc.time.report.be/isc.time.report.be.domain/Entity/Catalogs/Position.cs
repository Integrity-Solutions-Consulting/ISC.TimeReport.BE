using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class Position : BaseEntity
    {
        public string PositionName { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string? Description { get; set; }
    }
}
