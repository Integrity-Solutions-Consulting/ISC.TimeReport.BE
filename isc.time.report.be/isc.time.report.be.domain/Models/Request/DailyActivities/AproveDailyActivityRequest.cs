using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.DailyActivities
{
    public class AproveDailyActivityRequest
    {
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public List<int> ActivityId { get; set; }
        public DateTime DateInicio { get; set; }
        public DateTime DateFin { get; set; }
    }
}
