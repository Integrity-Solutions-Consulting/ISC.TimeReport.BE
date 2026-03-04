using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Holidays
{
    public class Holiday : BaseEntity
    {
        public string HolidayName { get; set; } = null!;
        public DateOnly HolidayDate { get; set; } // Formato DATEEE DEBE SER
        public bool IsRecurring { get; set; } = true;
        public string HolidayType { get; set; } = "NATIONAL";
        public string? Description { get; set; }
    }
}
