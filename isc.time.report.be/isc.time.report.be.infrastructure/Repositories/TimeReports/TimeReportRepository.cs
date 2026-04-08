using isc.time.report.be.application.Interfaces.Repository.TimeReports;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Holidays;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Response.Dashboards;
using isc.time.report.be.infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.TimeReports
{
    public class TimeReportRepository : ITimeReportRepository
    {
        private readonly DBContext _dbContext;

        public TimeReportRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<int>> GetProjectIdsForEmployeeByClientAsync(int employeeId, int clientId)
        {
            if (employeeId <= 0)
            {
                throw new ClientFaultException("El ID del empleado no puede ser menor o igual a 0.");
            }

            if (clientId <= 0)
            {
                throw new ClientFaultException("El ID del cliente no puede ser menor o igual a 0.");
            }
            var projectIds = await _dbContext.EmployeeProjects
                .Where(ep => ep.EmployeeID == employeeId && ep.Project.ClientID == clientId)
                .Select(ep => ep.ProjectID)
                .ToListAsync();
            if (!projectIds.Any())
            {
                throw new ServerFaultException(
                    $"No se encontraron proyectos para el empleado con ID {employeeId} y cliente con ID {clientId}."
                );
            }

            return projectIds;
        }

        public async Task<List<DailyActivity>> GetActivitiesByEmployeeAndProjectsAsync(
            int employeeId,
            List<int> projectIds,
            int year,
            int month,
            bool fullMonth)
        {
            var startDate = new DateOnly(year, month, 1);
            var endDate = fullMonth
                ? startDate.AddMonths(1).AddDays(-1)
                : new DateOnly(year, month, 15);

            var activity = await _dbContext.DailyActivities
                .Include(a => a.ActivityType)
                .Include(a => a.Project)
                .Where(a => a.EmployeeID == employeeId
                    && projectIds.Contains(a.ProjectID ?? 0)
                    && a.Status
                    && a.ActivityDate >= startDate
                    && a.ActivityDate <= endDate)
                .OrderBy(a => a.ActivityDate)
                .ToListAsync();
            return activity;
        }

        public async Task<List<Holiday>> GetActiveHolidaysByMonthAndYearAsync(int month, int year)
        {
            var holiday = await _dbContext.Holidays
                .Where(h => h.Status == true && (
                    (h.IsRecurring && h.HolidayDate.Month == month) ||
                    (!h.IsRecurring && h.HolidayDate.Month == month && h.HolidayDate.Year == year)
                ))
                .ToListAsync();
            return holiday;
        }

        public async Task<List<DashboardRecursosPendientesDto>> GetRecursosTimeReportPendienteAsync(int? month = null, int? year = null, bool mesCompleto = false)
        {
            var parameters = new[]
            {
                    new SqlParameter("@Mes", month ?? (object)DBNull.Value),
                    new SqlParameter("@Anio", year ?? (object)DBNull.Value),
                    new SqlParameter("@MesCompleto", mesCompleto ? 1 : 0)
                };

            var list = await _dbContext.Set<DashboardRecursosPendientesDto>()
                .FromSqlRaw("EXEC dbo.sp_RecursosTimeReportPendiente @Mes, @Anio, @MesCompleto", parameters)
                .ToListAsync();

            if (!list.Any())
            {
                throw new ServerFaultException("No se encontraron recursos pendientes para el Time Report");
            }

            var employeeIds = list.Select(x => x.EmployeeID).Distinct().ToList();

            var employeePhones = await _dbContext.Employees
                .Include(e => e.Person)
                .Where(e => employeeIds.Contains(e.Id))
                .ToDictionaryAsync(e => e.Id, e => e.Person.Phone);

            var activeProjectsByEmployee = await _dbContext.EmployeeProjects
                .Include(ep => ep.Project)
                    .ThenInclude(p => p.Client)
                .Where(ep => employeeIds.Contains(ep.EmployeeID)
                          && ep.Project != null && ep.Project.Status == true
                          && ep.Project.Client != null && ep.Project.Client.Status == true)
                .ToListAsync();

            var projectGroups = activeProjectsByEmployee
                .GroupBy(ep => ep.EmployeeID)
                .ToDictionary(g => g.Key, g => new
                {
                    ProyectosAsignados = string.Join(" - ", g.Select(x => x.Project.Name).Distinct()),
                    ClientesAsociados = string.Join(" - ", g.Select(x => x.Project.Client.TradeName ?? x.Project.Client.LegalName).Distinct()),
                    ClienteIDs = string.Join(",", g.Select(x => x.Project.ClientID).Distinct())
                });

            return list.Select(item => 
            {
                var dictValues = projectGroups.GetValueOrDefault(item.EmployeeID);
                return item with 
                {
                    Phone = employeePhones.GetValueOrDefault(item.EmployeeID) ?? string.Empty,
                    ProyectosAsignados = dictValues?.ProyectosAsignados ?? string.Empty,
                    ClientesAsociados = dictValues?.ClientesAsociados ?? string.Empty,
                    ClienteIDs = dictValues?.ClienteIDs ?? string.Empty
                };
            }).ToList();
        }

        public async Task<List<DashboardRecursosPendientesDto>> GetRecursosTimeReportPendienteFiltradoAsync(int? month = null, int? year = null, bool mesCompleto = false, byte bancoGuayaquil = 0)
        {
            var parameters = new[]
            {
                    new SqlParameter("@Mes", month ?? (object)DBNull.Value),
                    new SqlParameter("@Anio", year ?? (object)DBNull.Value),
                    new SqlParameter("@MesCompleto", mesCompleto ? 1 : 0),
                    new SqlParameter("@BancoGuayaquil", bancoGuayaquil)
                };

            var list = await _dbContext.Set<DashboardRecursosPendientesDto>()
                .FromSqlRaw("EXEC dbo.sp_RecursosTimeReportPorClienteBG @Mes, @Anio, @MesCompleto, @BancoGuayaquil", parameters)
                .ToListAsync();



            if (!list.Any())
            {
                throw new ServerFaultException("No se encontraron recursos pendientes para el Time Report");
            }

            var employeeIds = list.Select(x => x.EmployeeID).Distinct().ToList();

            var employeePhones = await _dbContext.Employees
                .Include(e => e.Person)
                .Where(e => employeeIds.Contains(e.Id))
                .ToDictionaryAsync(e => e.Id, e => e.Person.Phone);

            var activeProjectsByEmployee = await _dbContext.EmployeeProjects
                .Include(ep => ep.Project)
                    .ThenInclude(p => p.Client)
                .Where(ep => employeeIds.Contains(ep.EmployeeID)
                          && ep.Project != null && ep.Project.Status == true
                          && ep.Project.Client != null && ep.Project.Client.Status == true)
                .ToListAsync();

            var projectGroups = activeProjectsByEmployee
                .GroupBy(ep => ep.EmployeeID)
                .ToDictionary(g => g.Key, g => new
                {
                    ProyectosAsignados = string.Join(" - ", g.Select(x => x.Project.Name).Distinct()),
                    ClientesAsociados = string.Join(" - ", g.Select(x => x.Project.Client.TradeName ?? x.Project.Client.LegalName).Distinct()),
                    ClienteIDs = string.Join(",", g.Select(x => x.Project.ClientID).Distinct())
                });

            return list.Select(item => 
            {
                var dictValues = projectGroups.GetValueOrDefault(item.EmployeeID);
                return item with 
                {
                    Phone = employeePhones.GetValueOrDefault(item.EmployeeID) ?? string.Empty,
                    ProyectosAsignados = dictValues?.ProyectosAsignados ?? string.Empty,
                    ClientesAsociados = dictValues?.ClientesAsociados ?? string.Empty,
                    ClienteIDs = dictValues?.ClienteIDs ?? string.Empty
                };
            }).ToList();
        }

    }
}
