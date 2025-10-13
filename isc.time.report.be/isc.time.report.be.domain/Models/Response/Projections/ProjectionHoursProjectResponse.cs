using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Projections
{
    public record ProjectionHoursProjectResponse(

    int ProjectionHoursProjectId,
    int ResourceTypeId,
    string ResourceTypeName,
    string resource_name,
    decimal hourly_cost,
    int resource_quantity,
    string time_distribution,
    decimal total_time,
    decimal resource_cost,
    decimal participation_percentage,
    bool period_type,  
    int period_quantity,
    int ProjectID    

    );
}
