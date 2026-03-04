namespace isc.time.report.be.domain.Models.Response.Projects
{
    public class GetEmployeeProjectResponse
    {
        public int Id { get; set; }
        public int? EmployeeID { get; set; }
        public int? SupplierID { get; set; }
        public string AssignedRole { get; set; }
        public decimal? CostPerHour { get; set; }
        public decimal? AllocatedHours { get; set; }
        public int ProjectID { get; set; }
        public bool Status { get; set; }
    }
}
