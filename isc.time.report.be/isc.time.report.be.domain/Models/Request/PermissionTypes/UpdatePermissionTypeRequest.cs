using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.PermissionTypes
{
    public class UpdatePermissionTypeRequest
    {
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string? Description { get; set; }
        public bool IsPaid { get; set; }
        public bool RequiresApproval { get; set; }
        public int? MaxDays { get; set; }
        public bool Status { get; set; }
    }
}
