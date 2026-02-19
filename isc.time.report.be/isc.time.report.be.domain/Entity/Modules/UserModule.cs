using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Modules
{
    public class UserModule : BaseEntity
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public int ModuleID { get; set; }
        public Module Module { get; set; }
        public bool CanView { get; set; } = true;
        public bool CanCreate { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
    }
}
