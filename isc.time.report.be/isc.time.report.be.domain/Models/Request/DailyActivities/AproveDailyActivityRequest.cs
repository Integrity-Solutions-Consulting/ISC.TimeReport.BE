namespace isc.time.report.be.domain.Models.Request.DailyActivities
{
    public class AproveDailyActivityRequest
    {
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public List<int>? ActivityId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
