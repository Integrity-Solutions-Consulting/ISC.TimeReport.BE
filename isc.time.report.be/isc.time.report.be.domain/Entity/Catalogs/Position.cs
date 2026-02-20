using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class Position : BaseEntity
    {
        public string PositionName { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string? Description { get; set; }
    }
}
