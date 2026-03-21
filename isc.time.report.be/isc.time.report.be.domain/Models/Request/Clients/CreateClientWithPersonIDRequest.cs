namespace isc.time.report.be.domain.Models.Request.Clients
{
    public class CreateClientWithPersonIDRequest
    {
        public int? PersonID { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? Company { get; set; }
    }
}
