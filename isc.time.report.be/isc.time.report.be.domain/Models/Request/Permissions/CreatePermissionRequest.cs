namespace isc.time.report.be.domain.Models.Request.Permissions
{
    public class CreatePermissionRequest
    {
        public int PermissionTypeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsPaid { get; set; } = true;
        public string? Description { get; set; }
    }
}
