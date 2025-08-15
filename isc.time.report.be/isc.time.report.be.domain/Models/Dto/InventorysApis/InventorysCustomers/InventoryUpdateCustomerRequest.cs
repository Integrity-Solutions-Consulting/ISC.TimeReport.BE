using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysCustomers
{
    public class InventoryUpdateCustomerRequest
    {
        public string name { get; set; } = default!;
        public string address { get; set; } = default!;
        public string email { get; set; } = default!;
        public string phone { get; set; } = default!;
        public string ruc { get; set; } = default!;

    }
}
