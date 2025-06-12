using isc.time.report.be.domain.Entity.Activities;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.DailyActivities
{
    public class DailyActivity : BaseEntity
    {
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public int? ProjectID { get; set; }
        public Project Project { get; set; }
        public int ActivityTypeID { get; set; }
        public ActivityType ActivityType { get; set; }
        public decimal HoursQuantity { get; set; }
        public DateOnly ActivityDate { get; set; }
        public string ActivityDescription { get; set; } = null!;
        public string? Notes { get; set; }
        public bool IsBillable { get; set; } = true;
        public int? ApprovedByID { get; set; }        //CLAVE FORANEA DE QUIEN LO APROBO
        public User ApprovedByUser { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
