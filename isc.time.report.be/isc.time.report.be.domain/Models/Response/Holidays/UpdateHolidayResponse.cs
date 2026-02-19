namespace isc.time.report.be.domain.Models.Response.Holidays
{
    public class UpdateHolidayResponse
    {
        public int Id { get; set; }
        public string HolidayName { get; set; }
        public DateOnly HolidayDate { get; set; } // Formato DATEEE DEBE SER
        public bool IsRecurring { get; set; }
        public string HolidayType { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
