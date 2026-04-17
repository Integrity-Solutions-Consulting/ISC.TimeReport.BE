using isc.time.report.be.application.Interfaces.Repository.Homologaciones;
using isc.time.report.be.application.Interfaces.Service.Homologacion;
using isc.time.report.be.domain.Entity.Homologaciones;
using isc.time.report.be.domain.Models.Request.Homologacion;
using isc.time.report.be.domain.Models.Response.Homologacion;

namespace isc.time.report.be.application.Services.Homologaciones
{
    public class HomologacionService : IHomologacionService
    {
        private readonly IHomologacionRepository _repository;

        public HomologacionService(IHomologacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<HomologacionResponse>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            return entities.Select(h => new HomologacionResponse
            {
                Id = h.Id,
                NombreExterno = h.NombreExterno,
                EmployeeID = h.EmployeeID,
                NombreColaboradorTMR = h.Employee?.Person != null
                    ? $"{h.Employee.Person.FirstName} {h.Employee.Person.LastName}"
                    : $"Empleado #{h.EmployeeID}",
                Estado = h.Status
            }).ToList();
        }

        public async Task<HomologacionResponse> CreateAsync(CreateHomologacionRequest request)
        {
            var entity = new Homologacion
            {
                NombreExterno = request.NombreExterno.Trim(),
                EmployeeID = request.EmployeeID,
                Status = true,
                CreationUser = "SYSTEM",
                CreationDate = DateTime.Now,
                CreationIp = "0.0.0.0"
            };

            var created = await _repository.CreateAsync(entity);

            // Recargar con navegación para tener el nombre del colaborador
            var reloaded = await _repository.GetAllAsync();
            var result = reloaded.FirstOrDefault(h => h.Id == created.Id);

            return new HomologacionResponse
            {
                Id = result?.Id ?? created.Id,
                NombreExterno = result?.NombreExterno ?? created.NombreExterno,
                EmployeeID = result?.EmployeeID ?? created.EmployeeID,
                NombreColaboradorTMR = result?.Employee?.Person != null
                    ? $"{result.Employee.Person.FirstName} {result.Employee.Person.LastName}"
                    : $"Empleado #{created.EmployeeID}",
                Estado = result?.Status ?? created.Status
            };
        }
    }
}
