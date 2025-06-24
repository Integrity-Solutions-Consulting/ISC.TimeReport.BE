using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Response.Leaders;

namespace isc.time.report.be.domain.Models.Response.Leaders
{
    public class GetLeaderResponseXXX
    {
        public List<GetLeaderListResponseXXX> Leaders { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
