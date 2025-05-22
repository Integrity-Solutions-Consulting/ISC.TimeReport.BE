using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Customers
{
    public class GetRequest
    {
        public int IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string CommercialName { get; set; }
        public string CompanyName { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
