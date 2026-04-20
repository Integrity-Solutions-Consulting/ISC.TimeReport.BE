using isc.time.report.be.application.Interfaces.Repository.Homologaciones;
using isc.time.report.be.domain.Entity.Homologaciones;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.Homologaciones
{
    public class HomologacionRepository : IHomologacionRepository
    {
        private readonly DBContext _dbContext;

        public HomologacionRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Homologacion>> GetAllAsync()
        {
            return await _dbContext.Homologaciones
                .Include(h => h.Employee)
                    .ThenInclude(e => e.Person)
                .OrderBy(h => h.NombreExterno)
                .ToListAsync();
        }

        public async Task<Homologacion> CreateAsync(Homologacion entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.Status = true;
            entity.CreationUser = "SYSTEM";
            entity.CreationIp = "0.0.0.0";

            await _dbContext.Homologaciones.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Homologacion?> GetByNombreExternoAsync(string nombreExterno)
        {
            if (string.IsNullOrWhiteSpace(nombreExterno))
                return null;

            var normalized = nombreExterno.Trim().ToLower();

            return await _dbContext.Homologaciones
                .Include(h => h.Employee)
                .FirstOrDefaultAsync(h =>
                    h.Status == true &&
                    h.NombreExterno.ToLower() == normalized);
        }
    }
}
