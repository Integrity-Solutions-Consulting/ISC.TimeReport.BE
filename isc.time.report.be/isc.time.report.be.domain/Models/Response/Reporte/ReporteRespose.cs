using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Reporte
{
    public class ReporteRespose
    {

        public record ReporteRecursosPorProyectoDto(
             string NombreCliente,
             DateTime FechaInicio,
             DateTime FechaFin,
             string LiderProyecto,
             string NombreRecurso,
             string Cargo
        );

        public record CantidadRecursoHoraClienteDto(
            string Cliente,
            int MesNumero,
            int Año,
            int Cantidad_Recursos,
            decimal TotalHoras
        );


    }
}
