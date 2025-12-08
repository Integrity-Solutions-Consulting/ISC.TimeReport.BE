using isc.time.report.be.application.Interfaces.Repository.Reportes;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static isc.time.report.be.domain.Models.Response.Reporte.ReporteRespose;

namespace isc.time.report.be.infrastructure.Repositories.Reportes
{
    public class ReporteRepository : IReportesRepository
    {
        private readonly DBContext _dbContext;


        public ReporteRepository (DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CantidadRecursoHoraClienteDto>> ObtenerCantidadRecursosYHorasPorClienteAsync()
        {
            var resultado =  await _dbContext
                .Set<CantidadRecursoHoraClienteDto>()
                .FromSqlRaw("EXEC dbo.sp_CantidadRecursosYHorasPorCliente")
                .ToListAsync();

            return resultado;
        }

        public async Task<List<ReporteRecursosPorProyectoDto>> ObtenerReporteRecursosAsync()
        {
            var resultado = await _dbContext
                .Set<ReporteRecursosPorProyectoDto>()
                .FromSqlRaw("EXEC dbo.sp_ReporteRecursosPorProyecto")
                .ToListAsync();

            return resultado;
        }



    }
}
