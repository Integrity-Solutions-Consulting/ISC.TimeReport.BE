namespace isc.time.report.be.domain.Models.Response.DailyActivities
{
    public class CreateDailyActivityFromBGResponse
    {
        public string Type { get; set; }
        public string? Title { get; set; }
        public string Comment { get; set; }
        public string? RequirementCode { get; set; }
        public string Date { get; set; }
        public string Username { get; set; }
        public string Hours { get; set; }
        public string EmployeeCode { get; set; }
        public string Status { get; set; }
    }
}
