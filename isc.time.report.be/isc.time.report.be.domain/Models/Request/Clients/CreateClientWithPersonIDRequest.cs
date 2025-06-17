using isc.time.report.be.domain.Entity.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Clients
{
    public class CreateClientWithPersonIDRequest
    {
        public int? PersonID { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
    }
}
