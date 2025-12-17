using isc.time.report.be.application.Interfaces.Repository.Report;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static isc.time.report.be.domain.Models.Response.Report.ReportResponse;

namespace isc.time.report.be.infrastructure.Repositories.Report
{
    public class ReportRepository : IReportRepository
    {
        private readonly DBContext _dbContext;


        public ReportRepository (DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ClientHourlyResourceAmountDto>> GetResourceAndHoursCountByClientAsync()
        {
            var resultado =  await _dbContext
                .Set<ClientHourlyResourceAmountDto>()
                .FromSqlRaw("EXEC dbo.sp_TRClientResourcesAndHoursSummary")
                .ToListAsync();

            return resultado;
        }

        public async Task<List<ProjectResourcesReportDto>> GetResourcesReportAsync()
        {
            var resultado = await _dbContext
                .Set<ProjectResourcesReportDto>()
                .FromSqlRaw("EXEC dbo.sp_TRProjectResourcesReport")
                .ToListAsync();

            return resultado;
        }



    }
}
