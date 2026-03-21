using isc.time.report.be.domain.Models.Request.Persons;

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
