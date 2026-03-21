using isc.time.report.be.domain.Models.Response.Dashboards;

namespace isc.time.report.be.application.Interfaces.Service.Dashboards
{
    public interface IDashboardService
    {
        Task<DashboardResumenGeneralDto?> GetDashboardResumenGeneralAsync();
        Task<List<DashboardHorasActividadDto>> GetHorasPorActividadPorFechaAsync(DateOnly? fecha);
        Task<List<DashboardRecursosClienteDto>> GetRecursosPorClienteAsync();
        Task<List<DashboardResumenProyectoDto>> GetResumenProyectosAsync(string? tipoFiltro, string? valor);
    }
}
