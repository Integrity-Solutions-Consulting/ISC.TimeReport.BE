using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Auth
{
    public class UserRole : BaseEntity
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
    }

}
