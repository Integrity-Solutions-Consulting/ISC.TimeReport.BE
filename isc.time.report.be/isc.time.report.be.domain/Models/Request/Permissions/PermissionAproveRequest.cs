using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Permissions
{
    public class PermissionAproveRequest
    {
        public int PermissionID { get; set; }
        public string ApprovalStatus { get; set; }
        public string? Observation { get; set; }
    }
}
