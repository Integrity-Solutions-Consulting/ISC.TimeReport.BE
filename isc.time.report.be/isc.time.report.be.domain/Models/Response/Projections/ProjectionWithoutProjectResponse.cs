using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Projections
{
    public class ProjectionWithoutProjectResponse
    {
        public Guid Id { get; set; }
        public int ResourceTypeId { get; set; }
        public string ResourceName { get; set; } = string.Empty;
        public decimal HourlyCost { get; set; }
        public int ResourceQuantity { get; set; }
        public double TotalTime { get; set; }
        public decimal ResourceCost { get; set; }
        public double ParticipationPercentage { get; set; }
        public string PeriodType { get; set; } = string.Empty;
        public int PeriodQuantity { get; set; }
        public List<double> TimeDistribution { get; set; } = new();
    }
}
