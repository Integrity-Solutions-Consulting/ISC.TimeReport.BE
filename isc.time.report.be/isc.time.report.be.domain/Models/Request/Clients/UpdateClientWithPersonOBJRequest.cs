using isc.time.report.be.domain.Models.Request.Persons;

namespace isc.time.report.be.domain.Models.Request.Clients
{
    public class UpdateClientWithPersonOBJRequest
    {
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? Company { get; set; }
        public UpdatePersonRequest Person { get; set; }
    }
}
