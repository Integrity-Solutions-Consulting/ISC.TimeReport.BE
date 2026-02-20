using isc.time.report.be.domain.Models.Response.Persons;

namespace isc.time.report.be.domain.Models.Response.Clients
{
    public class CreateClientResponse
    {
        public int Id { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? Company { get; set; }
        public CreatePersonResponse person { get; set; }
        public bool Status { get; set; }
    }
}
