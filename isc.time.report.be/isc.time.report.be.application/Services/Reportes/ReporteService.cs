using isc.time.report.be.application.Interfaces.Repository.Reportes;
using isc.time.report.be.application.Interfaces.Service.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static isc.time.report.be.domain.Models.Response.Reporte.ReporteRespose;

namespace isc.time.report.be.application.Services.Reportes
{
    public class ReporteService : IReporteService
    {
        private readonly IReportesRepository _repo;

        public ReporteService(IReportesRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ReporteRecursosPorProyectoDto>> ObtenerReporteProyectoAsync()
        {
            var data = await _repo.ObtenerReporteRecursosAsync();
            return data;

        }

        public async Task<List<CantidadRecursoHoraClienteDto>> ObtenerReporteClienteAsync()
        {
            var data = await _repo.ObtenerCantidadRecursosYHorasPorClienteAsync();
            return data;
        }

    }
}
