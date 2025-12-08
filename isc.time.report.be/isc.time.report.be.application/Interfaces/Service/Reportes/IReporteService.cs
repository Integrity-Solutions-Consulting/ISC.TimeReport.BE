using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static isc.time.report.be.domain.Models.Response.Reporte.ReporteRespose;

namespace isc.time.report.be.application.Interfaces.Service.Reportes
{
    public interface IReporteService
    {
        Task<List<ReporteRecursosPorProyectoDto>> ObtenerReporteProyectoAsync();
        Task<List<CantidadRecursoHoraClienteDto>> ObtenerReporteClienteAsync();

    }
}
