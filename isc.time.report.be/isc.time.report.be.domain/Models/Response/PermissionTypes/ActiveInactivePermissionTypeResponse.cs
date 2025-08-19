using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.PermissionTypes
{
    public class ActiveInactivePermissionTypeResponse
    {
        public int Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string? Description { get; set; }
        public bool IsPaid { get; set; }
        public bool RequiresApproval { get; set; }
        public int? MaxDays { get; set; }
        public bool Status { get; set; }
    }
}
