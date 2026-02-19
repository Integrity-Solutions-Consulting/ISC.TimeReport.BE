using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class ProjectStatus : BaseEntity
    {
        public string StatusCode { get; set; } = null!;
        public string StatusName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
