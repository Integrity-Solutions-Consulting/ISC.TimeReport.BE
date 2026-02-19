using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Historycs
{
    public class Historic : BaseEntity
    {
        public int DailyActivityID { get; set; }
        public DailyActivity DailyActivity { get; set; }
        public string ChangeType { get; set; } = null!;
        public TimeOnly? OldStartTime { get; set; }
        public TimeOnly? OldEndTime { get; set; }
        public string? OldDescription { get; set; }
        public decimal? OldHours { get; set; } = 0;
        public TimeOnly? NewStartTime { get; set; }
        public TimeOnly? NewEndTime { get; set; }
        public string? NewDescription { get; set; }
        public decimal? NewHours { get; set; } = 0;
        public string? ChangeReason { get; set; }
        public string ChangedBy { get; set; } = null!;
        public DateTime ChangeDate { get; set; } = DateTime.Now;
        public string? ChangeIP { get; set; }
    }

}
