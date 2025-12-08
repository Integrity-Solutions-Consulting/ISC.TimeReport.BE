
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static isc.time.report.be.domain.Models.Response.Reporte.ReporteRespose;

namespace isc.time.report.be.application.Interfaces.Repository.Reportes
{
    public interface IReportesRepository
    {

        Task<List<ReporteRecursosPorProyectoDto>> ObtenerReporteRecursosAsync();
        Task<List<CantidadRecursoHoraClienteDto>> ObtenerCantidadRecursosYHorasPorClienteAsync();


    }
}
