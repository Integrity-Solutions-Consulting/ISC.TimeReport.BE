using isc.time.report.be.domain.Models.Response.Employees;

namespace isc.time.report.be.domain.Models.Response.Projects
{
    public class GetProjectDetailByIDResponse
    {
        public int Id { get; set; }
        public int ClientID { get; set; }
        public int ProjectStatusID { get; set; }
        public int? ProjectTypeID { get; set; }
        public int? LeaderID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal Hours { get; set; }
        public decimal? Budget { get; set; }
        public DateTime? WaitingStartDate { get; set; }
        public DateTime? WaitingEndDate { get; set; }
        public string? Observation { get; set; }
        public Lider? Leader { get; set; }

        public List<GetEmployeeProjectResponse> EmployeeProjects { get; set; }
        public List<GetEmployeesPersonInfoResponse> EmployeesPersonInfo { get; set; }
    }
}
