namespace isc.time.report.be.domain.Models.Response.Employees
{
    public class EmployeeProjectDetailDto
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; } = null!;
        public int ClientID { get; set; }
        public string ClientName { get; set; } = null!;
    }
}
