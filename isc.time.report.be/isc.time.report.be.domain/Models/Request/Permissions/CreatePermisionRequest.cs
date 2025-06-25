using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Permisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Permissions
{
    public class CreatePermisionRequest
    {
        public int PermissionTypeID { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsPaid { get; set; } = true;
        public string? Description { get; set; }
    }
}
