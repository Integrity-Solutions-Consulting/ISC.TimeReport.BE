using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Customers
{
    public class CustomersRequest
    {
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string Identification { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


    }
}
