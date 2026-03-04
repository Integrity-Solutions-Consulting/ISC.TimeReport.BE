namespace isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysCustomers
{
    public class InventoryCreateCustomerRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Ruc { get; set; }
    }
}
