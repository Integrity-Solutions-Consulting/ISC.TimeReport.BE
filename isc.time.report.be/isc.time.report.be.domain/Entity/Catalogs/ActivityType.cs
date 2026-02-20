using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class ActivityType : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ColorCode { get; set; }
    }
}
