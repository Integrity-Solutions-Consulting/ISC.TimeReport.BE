using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Projects
{
    public class Project : BaseEntity
    {
        public int ClientID { get; set; }
        public Client Client { get; set; }
        public int ProjectStatusID { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public int? ProjectTypeID { get; set; }
        public ProjectType ProjectType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? Budget { get; set; }
        public decimal Hours { get; set; }
        public List<EmployeeProject> EmployeeProject { get; set; }
    }
}
