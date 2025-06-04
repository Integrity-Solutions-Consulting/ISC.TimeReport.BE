using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Persons;

namespace isc.time.report.be.domain.Models.Request.Leaders
{
    public class GetLeaderRequest
    {
        public string Id { get; }
        public string LeaderType { get; set; }
        public string ProjectCode { get; set; }
        public string CustomerCode { get; set; }
        public string IdPerson { get; set; }
        public Person Person { get; set; }
    }
}
