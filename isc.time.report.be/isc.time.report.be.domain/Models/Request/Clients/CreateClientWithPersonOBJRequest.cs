using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Models.Request.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Clients
{
    public class CreateClientWithPersonOBJRequest
    {
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? Company { get; set; }
        public CreatePersonRequest Person { get; set; }
    }
}
