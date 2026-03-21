using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Clients
{
    public class Client : BaseEntity
    {
        public int? PersonID { get; set; }
        public Person? Person { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? Company { get; set; }
    }
}