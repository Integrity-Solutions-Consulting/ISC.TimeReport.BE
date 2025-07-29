using isc.time.report.be.application.Interfaces.Repository.Dashboards;
using isc.time.report.be.domain.Models.Response.Dashboards;
using isc.time.report.be.infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Dashboards
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DBContext _context;
        public DashboardRepository(DBContext context)
        {
            _context = context;
        }
        public async Task<DashboardResumenGeneralDto?> GetDashboardResumenGeneralAsync()
        {
            var result = await _context.Set<DashboardResumenGeneralDto>()
                .FromSqlRaw("SELECT * FROM dbo.fn_DashboardResumenGeneral()")
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<DashboardHorasActividadDto>> GetHorasPorActividadPorFechaAsync(DateOnly? fecha)
        {
            var dateParam = new SqlParameter("@fecha", fecha.HasValue ? fecha.Value.ToDateTime(new TimeOnly(0)) : DBNull.Value);
            return await _context.Set<DashboardHorasActividadDto>().FromSqlRaw("EXEC dbo.sp_HorasPorActividadPorFecha @fecha", dateParam).ToListAsync();
        }

        public async Task<List<DashboardRecursosClienteDto>> GetRecursosPorClienteAsync()
        {
            return await _context.Set<DashboardRecursosClienteDto>().FromSqlRaw("SELECT * FROM dbo.fn_RecursosPorCliente()").ToListAsync();
        }

        public async Task<List<DashboardResumenProyectoDto>> GetResumenProyectosAsync(string? tipoFiltro, string? valor)
        {
            var param1 = new SqlParameter("@TipoFiltro", tipoFiltro ?? (object)DBNull.Value);
            var param2 = new SqlParameter("@Valor", valor ?? (object)DBNull.Value);
            return await _context.Set<DashboardResumenProyectoDto>().FromSqlRaw("SELECT * FROM dbo.fn_ResumenProyectos(@TipoFiltro, @Valor)", param1, param2).ToListAsync();
        }


    }
}
