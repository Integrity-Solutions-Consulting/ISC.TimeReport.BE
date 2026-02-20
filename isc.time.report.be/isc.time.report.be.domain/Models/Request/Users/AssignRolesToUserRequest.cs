namespace isc.time.report.be.domain.Models.Request.Users
{
    public class AssignRolesToUserRequest
    {
        public int UserID { get; set; }
        public List<int> RolesIDs { get; set; } = new();
    }
}
