using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class ApprovalStatus : BaseEntity
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string? Description { get; set; }
    }
}
