using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Catalogs
{
    public class ProjectType : BaseEntity
    {
        public string TypeName { get; set; } = null!;
        public bool? SubType { get; set; }
    }
}
