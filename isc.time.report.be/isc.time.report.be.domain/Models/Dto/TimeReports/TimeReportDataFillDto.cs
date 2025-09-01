using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Dto.TimeReports
{
    public class TimeReportDataFillDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TradeName { get; set; }
        public string? Company { get; set; }
        public bool? EmployeeCompany { get; set; }


        public List<TimeReportActivityDto> Activities { get; set; }

    }

    public class TimeReportActivityDto
    {
        public int LeaderId { get; set; }
        public string LeaderName { get; set; }
        public int ActivityTypeID { get; set; }
        public ActivityType ActivityType { get; set; }
        public decimal HoursQuantity { get; set; }
        public DateOnly ActivityDate { get; set; }
        public string ActivityDescription { get; set; }
        public string? Notes { get; set; }
        public string RequirementCode { get; set; }
    }

}
