namespace isc.time.report.be.domain.Models.Response.Users
{
    public class GetAllUsersResponse
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool? IsActive { get; set; }
        public List<RoleResponse> Role { get; set; }
        public List<ModuleResponse> Modules { get; set; }
    }
}
