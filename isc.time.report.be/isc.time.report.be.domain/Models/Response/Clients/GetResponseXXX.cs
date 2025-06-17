using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Response.Clients;

namespace isc.time.report.be.domain.Models.Response.Clients
{
    public class GetResponseXXX
    {
        public List<CustomerListResponseXXX> Customers { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
