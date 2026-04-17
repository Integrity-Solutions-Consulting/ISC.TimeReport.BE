using isc.time.report.be.domain.Models.Request.Homologacion;
using isc.time.report.be.domain.Models.Response.Homologacion;

namespace isc.time.report.be.application.Interfaces.Service.Homologaciones
{
    public interface IHomologacionService
    {
        Task<List<HomologacionResponse>> GetAllAsync();
        Task<HomologacionResponse> CreateAsync(CreateHomologacionRequest request);
    }
}
