using isc.time.report.be.domain.Models.Response.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.Dashboards
{
    public interface IDashboardService
    {
        Task<List<DashboardHorasActividadDto>> GetHorasPorActividadPorFechaAsync(DateOnly? fecha);
        Task<List<DashboardRecursosClienteDto>> GetRecursosPorClienteAsync();
        Task<List<DashboardResumenProyectoDto>> GetResumenProyectosAsync(string? tipoFiltro, string? valor);
    }
}
