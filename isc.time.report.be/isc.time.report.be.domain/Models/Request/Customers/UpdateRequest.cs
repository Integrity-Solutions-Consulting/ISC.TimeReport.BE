using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Response.Shared;

namespace isc.time.report.be.domain.Models.Request.Customers
{
    public class UpdateRequest
    {
        public int Id { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string CommercialName { get; set; }
        public string CompanyName { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
