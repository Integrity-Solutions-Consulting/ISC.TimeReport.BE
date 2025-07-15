using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
