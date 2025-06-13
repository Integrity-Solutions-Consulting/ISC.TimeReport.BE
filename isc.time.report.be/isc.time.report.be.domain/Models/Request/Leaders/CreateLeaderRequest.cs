using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Leaders
{
    public class CreateLeaderRequest
    {
        public string LeaderType { get; set; }
        public string ProjectCode { get; set; }
        public string CustomerCode { get; set; }
        public string IdPerson { get; set; }
    }
}
