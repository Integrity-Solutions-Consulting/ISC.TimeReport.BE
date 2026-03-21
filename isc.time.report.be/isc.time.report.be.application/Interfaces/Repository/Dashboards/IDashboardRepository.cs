using isc.time.report.be.domain.Models.Response.Dashboards;

namespace isc.time.report.be.application.Interfaces.Repository.Dashboards
{
    public interface IDashboardRepository
    {
        Task<DashboardResumenGeneralDto?> GetDashboardResumenGeneralAsync();
        Task<List<DashboardHorasActividadDto>> GetHorasPorActividadPorFechaAsync(DateOnly? fecha);
        Task<List<DashboardRecursosClienteDto>> GetRecursosPorClienteAsync();
        Task<List<DashboardResumenProyectoDto>> GetResumenProyectosAsync(string? tipoFiltro, string? valor);
    }
}
