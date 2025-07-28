using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Projects
{
    public class UpdateProjectResponse
    {
        public int Id { get; set; }
        public int ClientID { get; set; }
        public int ProjectStatusID { get; set; }
        public int? ProjectTypeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? Budget { get; set; }
        public int Hours { get; set; }
        public bool? Status { get; set; }
    }
}
