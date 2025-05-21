using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Customers
{
    public class Bill : BaseEntity
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool status { get; set; }

    }
}
