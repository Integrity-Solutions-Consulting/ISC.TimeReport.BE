using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Persons;

namespace isc.time.report.be.domain.Models.Response.Clients
{
    public class CreateClientResponse
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string TradeName { get; set; }
        public string LegalName { get; set; }
        public Person Person { get; set; }
    }
}
