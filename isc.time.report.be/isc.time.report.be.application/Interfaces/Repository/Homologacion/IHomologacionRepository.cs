using isc.time.report.be.domain.Entity.Homologacion;

namespace isc.time.report.be.application.Interfaces.Repository.Homologacion
{
    public interface IHomologacionRepository
    {
        Task<List<Homologacion>> GetAllAsync();
        Task<Homologacion> CreateAsync(Homologacion entity);
        Task<Homologacion?> GetByNombreExternoAsync(string nombreExterno);
    }
}
