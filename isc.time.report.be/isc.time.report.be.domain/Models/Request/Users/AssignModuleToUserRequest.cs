namespace isc.time.report.be.domain.Models.Request.Users
{
    public class AssignModuleToUserRequest
    {
        public int UserID { get; set; }
        public List<int> ModuleIDs { get; set; } = new();
    }
}
