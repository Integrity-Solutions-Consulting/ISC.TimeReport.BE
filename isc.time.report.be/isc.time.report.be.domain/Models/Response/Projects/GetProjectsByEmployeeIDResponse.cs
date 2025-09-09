using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Entity.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Projects
{
    public class GetProjectsByEmployeeIDResponse
    {
        public int Id { get; set; }
        public int ClientID { get; set; }
        public int ProjectStatusID { get; set; }
        public int? ProjectTypeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Budget { get; set; }
        public decimal Hours { get; set; }
        public bool Status { get; set; }
        public DateTime? WaitingStartDate { get; set; }
        public DateTime? WaitingEndDate { get; set; }
        public string? Observation { get; set; }
    }
}
