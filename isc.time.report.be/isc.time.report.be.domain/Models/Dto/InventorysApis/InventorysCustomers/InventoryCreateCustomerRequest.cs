using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysCustomers
{
    public class InventoryCreateCustomerRequest
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;

        public string ruc {  get; set; } = default!;
    }
}
