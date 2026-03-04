namespace isc.time.report.be.domain.Models.Response.Auth
{
    public class RegisterRequest
    {
        public int EmployeeID { get; set; }
        public string Username { get; set; } = null!;
        public bool? IsActive { get; set; } = true;
        public List<int> RolesID { get; set; }
    }
}
