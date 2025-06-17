using isc.time.report.be.domain.Models.Response.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Clients
{
    public class UpdateClientResponse
    {
        public int Id { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public UpdatePersonResponse person { get; set; }
        public bool Status { get; set; }
    }
}
