namespace isc.time.report.be.domain.Models.Response.Homologacion
{
    public class HomologacionResponse
    {
        public int Id { get; set; }
        public string NombreExterno { get; set; } = null!;
        public int EmployeeID { get; set; }
        public string NombreColaboradorTMR { get; set; } = null!;
        public bool Estado { get; set; }
    }
}
