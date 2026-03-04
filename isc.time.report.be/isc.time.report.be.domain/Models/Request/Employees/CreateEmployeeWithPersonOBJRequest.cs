using isc.time.report.be.domain.Models.Request.Persons;

namespace isc.time.report.be.domain.Models.Request.Employees
{
    public class CreateEmployeeWithPersonOBJRequest
    {
        public CreatePersonRequest Person { get; set; }
        public int? PositionID { get; set; }
        public int WorkModeID { get; set; } = 1;
        public int EmployeeCategoryID { get; set; }
        public int CompanyCatalogID { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool? ContractType { get; set; }
        public int? DepartmentID { get; set; }
        public string? CorporateEmail { get; set; }
        public decimal? Salary { get; set; }
    }
}
