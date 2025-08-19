using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Permisions
{
    public class Permission : BaseEntity
    {
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public int PermissionTypeID { get; set; }
        public PermissionType PermissionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; } = 0;
        public decimal? TotalHours { get; set; } = 0;
        public bool IsPaid { get; set; } = true;
        public string? Description { get; set; }
        public int ApprovalStatusID { get; set; }
        public int? ApprovedByID { get; set; }
        public User ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? Observation { get; set; }
    }
}
