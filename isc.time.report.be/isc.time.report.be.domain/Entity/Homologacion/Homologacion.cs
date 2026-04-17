using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Homologacion
{
    public class Homologacion : BaseEntity
    {
        public int EmployeeID { get; set; }
        public string NombreExterno { get; set; } = null!;

        // Navegación
        public Employee Employee { get; set; } = null!;
    }
}
