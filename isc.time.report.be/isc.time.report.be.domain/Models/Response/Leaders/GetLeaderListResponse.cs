using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Persons;

namespace isc.time.report.be.domain.Models.Response.Leaders
{
    public class GetLeaderListResponse
    {
        public string Id { get; set; }
        public string LeaderType { get; set; }
        public string ProjectCode { get; set; }
        public string CustomerCode { get; set; }
        public string Id_Person { get; set; }
        public Person Person { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
    }
}
