namespace isc.time.report.be.domain.Models.Response.Employees
{
    public class GetEmployeesPersonInfoResponse
    {
        public int Id { get; set; }
        public int PersonID { get; set; }
        public int WorkModeID { get; set; }
        public int EmployeeCategoryID { get; set; }
        public int CompanyCatalogID { get; set; }
        public string EmployeeCode { get; set; }
        public string IdentificationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
    }
}
