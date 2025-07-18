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
}
