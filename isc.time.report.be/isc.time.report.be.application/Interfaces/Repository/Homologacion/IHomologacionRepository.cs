using isc.time.report.be.domain.Entity.Homologaciones;

namespace isc.time.report.be.application.Interfaces.Repository.Homologaciones
{
    public interface IHomologacionRepository
    {
        Task<List<Homologacion>> GetAllAsync();
        Task<Homologacion> CreateAsync(Homologacion entity);
        Task<Homologacion?> GetByNombreExternoAsync(string nombreExterno);
    }
}
