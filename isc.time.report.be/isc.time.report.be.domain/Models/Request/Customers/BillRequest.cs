using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Customers
{
    internal class BillRequest
    {

        public DateTime date { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
    }
}
