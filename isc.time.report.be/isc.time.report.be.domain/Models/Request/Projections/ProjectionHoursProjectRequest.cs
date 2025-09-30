using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Projections
{
    public class ProjectionHoursProjectRequest
    {
    public int ProjectionHoursProjectId { get; set; }
    public int ResourceTypeId { get; set; }
    public string ResourceTypeName { get; set;}
    public string resource_name { get; set; }
    public decimal hourly_cost { get; set; }
    public int resource_quantity { get; set; }
    public string time_distribution { get; set; }
    public decimal total_time { get; set; }
    public decimal resource_cost { get; set; }
    public decimal participation_percentage { get; set; }
    public bool period_type { get; set; }
    public int period_quantity { get; set; }
    public int ProjectID { get; set; }
    }
}
