using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Holidays
{
    public class ActiveInactiveHolidayResponse
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
