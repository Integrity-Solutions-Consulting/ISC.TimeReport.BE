using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Catalogs
{
    public class GetGenderResponse
    {
        public int Id { get; set; }
        public string GenderCode { get; set; }
        public string GenderName { get; set; }
    }
}
