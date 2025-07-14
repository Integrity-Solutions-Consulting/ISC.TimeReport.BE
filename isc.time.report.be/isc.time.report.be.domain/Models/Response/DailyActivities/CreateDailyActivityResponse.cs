using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.DailyActivities
{
    public class CreateDailyActivityResponse
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int? ProjectID { get; set; }
        public int ActivityTypeID { get; set; }
        public decimal HoursQuantity { get; set; }
        public DateOnly ActivityDate { get; set; }
        public string ActivityDescription { get; set; }
        public string? Notes { get; set; }
        public bool IsBillable { get; set; }
        public int? ApprovedByID { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string RequirementCode { get; set; }
        public bool Status { get; set; }
    }
}
