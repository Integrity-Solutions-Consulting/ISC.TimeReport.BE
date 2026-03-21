using isc.time.report.be.domain.Models.Response.Users;

namespace isc.time.report.be.domain.Models.Response.Auth
{
    public class GetRolesResponse
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public List<ModuleResponse> Modules { get; set; }
    }
}
