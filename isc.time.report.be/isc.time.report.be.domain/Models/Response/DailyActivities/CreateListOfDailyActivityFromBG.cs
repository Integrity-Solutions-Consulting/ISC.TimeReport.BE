namespace isc.time.report.be.domain.Models.Response.DailyActivities
{
    public class CreateListOfDailyActivityFromBG
    {
        public List<CreateDailyActivityFromBGResponse> Activities { get; set; } = new();
        public int TotalInserted => Activities.Count(a => a.Status == "Inserted");
        public int TotalErrors => Activities.Count(a => a.Status != "Inserted");
    }
}
