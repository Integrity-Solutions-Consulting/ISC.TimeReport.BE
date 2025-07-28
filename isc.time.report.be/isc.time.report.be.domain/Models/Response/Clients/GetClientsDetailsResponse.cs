using isc.time.report.be.domain.Models.Response.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Clients
{
    public class GetClientsDetailsResponse
    {
        public int Id { get; set; }
        public GetPersonResponse Person { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? Company { get; set; }
        public bool Status { get; set; }
    }
}
