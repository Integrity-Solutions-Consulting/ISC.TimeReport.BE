using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Employees
{
    public class Employee : BaseEntity
    {
        public int PersonID { get; set; }
        public Person Person { get; set; }
        public int? PositionID { get; set; }
        public Position Position { get; set; }
        public int? DepartmentID { get; set; }
        public Department Department { get; set; }
        public int WorkModeID { get; set; }
        public WorkMode WorkMode { get; set; }
        public int EmployeeCategoryID { get; set; }
        public int CompanyCatalogID { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public DateTime? HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool? ContractType { get; set; }
        public string? CorporateEmail { get; set; }
        public decimal? Salary { get; set; }
        public List<EmployeeProject> EmployeeProject { get; set; }
        public List<User> User { get; set; }
    }
}
