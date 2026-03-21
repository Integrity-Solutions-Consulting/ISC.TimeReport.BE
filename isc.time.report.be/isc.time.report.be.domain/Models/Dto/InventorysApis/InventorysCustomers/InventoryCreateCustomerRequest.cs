namespace isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysCustomers
{
    public class InventoryCreateCustomerRequest
    {
        public int id { get; set; }
        public string name { get; set; } = default!;
        public string address { get; set; } = default!;
        public string email { get; set; } = default!;
        public string phone { get; set; } = default!;
        public string ruc { get; set; } = default!;
    }
}
