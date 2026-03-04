using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Leaders
{
    public class Leader : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool LeadershipType { get; set; } = true;
    }
}
