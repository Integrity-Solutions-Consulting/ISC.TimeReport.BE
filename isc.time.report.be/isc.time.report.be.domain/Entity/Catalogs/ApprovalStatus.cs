using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class ApprovalStatus : BaseEntity
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string? Description { get; set; }
    }
}
