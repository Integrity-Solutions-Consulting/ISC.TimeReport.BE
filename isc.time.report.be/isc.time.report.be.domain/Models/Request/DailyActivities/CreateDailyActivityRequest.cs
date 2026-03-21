namespace isc.time.report.be.domain.Models.Request.DailyActivities
{
    public class CreateDailyActivityRequest
    {
        public int? ProjectID { get; set; }
        public int ActivityTypeID { get; set; }
        public string RequirementCode { get; set; }
        public decimal HoursQuantity { get; set; }
        public DateOnly ActivityDate { get; set; }
        public string ActivityDescription { get; set; }
        public string? Notes { get; set; }
    }
}
