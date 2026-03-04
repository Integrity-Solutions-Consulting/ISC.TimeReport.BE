using isc.time.report.be.application.Interfaces.Repository.Dashboards;
using isc.time.report.be.application.Interfaces.Service.Dashboards;
using isc.time.report.be.domain.Models.Response.Dashboards;

namespace isc.time.report.be.application.Services.Dashboards
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;
        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<DashboardResumenGeneralDto?> GetDashboardResumenGeneralAsync()
        {
            return await _repo.GetDashboardResumenGeneralAsync();
        }

        public async Task<List<DashboardHorasActividadDto>> GetHorasPorActividadPorFechaAsync(DateOnly? fecha)
        {
            return await _repo.GetHorasPorActividadPorFechaAsync(fecha);
        }

        public async Task<List<DashboardRecursosClienteDto>> GetRecursosPorClienteAsync()
        {
            return await _repo.GetRecursosPorClienteAsync();
        }

        public async Task<List<DashboardResumenProyectoDto>> GetResumenProyectosAsync(string? tipoFiltro, string? valor)
        {
            if (!string.IsNullOrWhiteSpace(tipoFiltro) && !new[] { "COLABORADOR", "PROYECTO", "CLIENTE" }.Contains(tipoFiltro.ToUpper()))
                throw new ArgumentException("Tipo de filtro no válido");

            return await _repo.GetResumenProyectosAsync(tipoFiltro?.ToUpper(), valor);
        }
    }
}
