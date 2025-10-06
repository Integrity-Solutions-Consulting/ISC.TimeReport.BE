using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Projections
{
    public class CreateProjectionHoursProjectResponse
    {
        public int ResourceTypeId { get; set; }
        public string ResourceName { get; set; }
        public decimal HourlyCost { get; set; }
        public int ResourceQuantity { get; set; }
        public List<double> TimeDistribution { get; set; }
        public decimal TotalTime { get; set; }
        public decimal ResourceCost { get; set; }
        public decimal ParticipationPercentage { get; set; }
        public bool PeriodType { get; set; }
        public int PeriodQuantity { get; set; }
        public int ProjecId { get; set; }
    }
}
