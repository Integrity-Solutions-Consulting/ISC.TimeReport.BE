using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Shared;

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
        public string? Code { get; set; } = null;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? Budget { get; set; } = 0;
        public decimal? Hours { get; set; } = 0;
        public List<EmployeeProject> EmployeeProject { get; set; }
        public int? LeaderID { get; set; }
        public Leader? Leader { get; set; }
        public DateTime? WaitingStartDate { get; set; }
        public DateTime? WaitingEndDate { get; set; }
        public string? Observation { get; set; }
    }
}
