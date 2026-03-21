namespace isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysLogin
{
    public class InventoryLoginDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class InventoryLoginData
    {
        public string username { get; set; }
        public string email { get; set; }
        public string firstNames { get; set; }
        public string token { get; set; }
    }

    public class InventoryLoginResponse
    {
        public object meta { get; set; }
        public InventoryLoginData data { get; set; }
    }

}
