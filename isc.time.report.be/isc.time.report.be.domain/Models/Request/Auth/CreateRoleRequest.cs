namespace isc.time.report.be.domain.Models.Request.Auth
{
    public class CreateRoleRequest
    {
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
        public List<int> ModuleIds { get; set; }
    }
}
