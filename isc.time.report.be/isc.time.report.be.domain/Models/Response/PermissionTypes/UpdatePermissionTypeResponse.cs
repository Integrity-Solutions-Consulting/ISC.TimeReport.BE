namespace isc.time.report.be.domain.Models.Response.PermissionTypes
{
    public class UpdatePermissionTypeResponse
    {
        public int Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string? Description { get; set; }
        public bool IsPaid { get; set; }
        public bool RequiresApproval { get; set; }
        public int? MaxDays { get; set; }
        public bool Status { get; set; }
    }
}
