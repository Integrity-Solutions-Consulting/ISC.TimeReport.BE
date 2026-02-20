namespace isc.time.report.be.domain.Models.Response.Clients
{
    public class GetClientsByEmployeeIDResponse
    {
        public int Id { get; set; }
        public int? PersonID { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? Company { get; set; }
    }
}
