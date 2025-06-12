using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Projects
{
    public class ProjectStatus : BaseEntity
    {
        public string StatusCode { get; set; } = null!;
        public string StatusName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
