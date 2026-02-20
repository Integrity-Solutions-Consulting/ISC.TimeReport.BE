using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class WorkMode : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; } = null;
    }
}
