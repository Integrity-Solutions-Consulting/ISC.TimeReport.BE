using isc.time.report.be.domain.Entity.Auth;
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
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public decimal? TotalHours { get; set; }
        public bool IsPaid { get; set; } = true;
        public string? Description { get; set; }
        public string ApprovalStatus { get; set; } = "PENDING";
        public int? ApprovedByID { get; set; }
        public User ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? Observation { get; set; }
    }
}
