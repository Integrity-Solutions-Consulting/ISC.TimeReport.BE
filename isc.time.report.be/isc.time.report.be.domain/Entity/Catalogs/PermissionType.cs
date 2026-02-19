using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class PermissionType : BaseEntity
    {
        public string TypeCode { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPaid { get; set; } = true;
        public bool RequiresApproval { get; set; } = true;
        public int? MaxDays { get; set; }
    }
}
