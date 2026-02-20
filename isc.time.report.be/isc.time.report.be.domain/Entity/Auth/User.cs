using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Auth
{
    public class User : BaseEntity
    {
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool? MustChangePassword { get; set; } = true;
        public List<UserRole> UserRole { get; set; }
        public List<UserModule> UserModule { get; set; }
    }
}
