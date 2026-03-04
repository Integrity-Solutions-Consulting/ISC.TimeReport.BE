using isc.time.report.be.domain.Models.Response.Users;

namespace isc.time.report.be.domain.Models.Response.Auth
{
    public class RegisterResponse
    {
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public bool? IsActive { get; set; }
        public bool? MustChangePassword { get; set; }
        public List<RoleResponse> Roles { get; set; }
    }
}
