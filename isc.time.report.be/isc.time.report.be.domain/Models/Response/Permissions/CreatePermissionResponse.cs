using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Permisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Permissions
{
    public class CreatePermissionResponse
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int PermissionTypeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public decimal? TotalHours { get; set; }
        public bool IsPaid { get; set; }
        public string? Description { get; set; }
        public string ApprovalStatus { get; set; }
        public int? ApprovedByID { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? Observation { get; set; }
        public bool Status { get; set; }
    }
}
