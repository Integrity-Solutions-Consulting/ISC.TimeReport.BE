using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Dashboards
{
    public record DashboardHorasActividadDto(string TipoActividad, decimal TotalHoras);
    public record DashboardRecursosClienteDto(string ClientName, int TotalRecursos, decimal Porcentaje);
    public record DashboardResumenProyectoDto(string Proyecto, string Cliente, string Lider_Tecnico, string Fecha_Inicio, string Fecha_Finalizacion, string Colaboradores);
    public record DashboardRecursosPendientesDto(int EmployeeID, string NombreCompletoEmpleado, decimal HorasRegistradasMes, int TotalDiasHabilesMes, int DiasConReporte, int DiasOchoHorasCumplidas, int DiasPendientes);

    public class DashboardResumenGeneralDto
    {
        public int TotalProyectosActivos { get; set; }
        public int TotalClientes { get; set; }
        public int TotalEmpleados { get; set; }
        public int ProyectosPlanificacion { get; set; }
        public int ProyectosAprobados { get; set; }
        public int ProyectosEnProgreso { get; set; }
        public int ProyectosEnEspera { get; set; }
        public int ProyectosCancelados { get; set; }
        public int ProyectosCompletos { get; set; }
        public int ProyectosAplazados { get; set; }
    }

}
