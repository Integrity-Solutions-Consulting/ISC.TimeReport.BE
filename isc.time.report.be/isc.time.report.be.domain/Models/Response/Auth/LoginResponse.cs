using isc.time.report.be.domain.Models.Response.Users;

namespace isc.time.report.be.domain.Models.Response.Auth
{
    public class LoginResponse
    {
        public int UserID { get; set; }
        public int EmployeeID { get; set; }
        public string TOKEN { get; set; }
        public bool MustChangePassword { get; set; }
        public List<RoleResponse> Roles { get; set; }
        public List<ModuleResponse> Modules { get; set; }
    }

}
