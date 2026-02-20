using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.ProjectionHours
{
    public class ProjectionHour : BaseEntity
    {
        public Guid? GroupProjection { get; set; }
        public int ResourceTypeId { get; set; }
        public string ResourceName { get; set; }
        public string ProjectionName { get; set; }
        public decimal HourlyCost { get; set; }
        public int ResourceQuantity { get; set; }
        public string TimeDistribution { get; set; }
        public decimal TotalTime { get; set; }
        public decimal ResourceCost { get; set; }
        public decimal ParticipationPercentage { get; set; }
        public bool PeriodType { get; set; }
        public int PeriodQuantity { get; set; }
    }
}
