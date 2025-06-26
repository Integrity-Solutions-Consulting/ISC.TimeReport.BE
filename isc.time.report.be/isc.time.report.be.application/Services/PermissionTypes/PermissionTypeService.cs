using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.PermissionTypes;
using isc.time.report.be.application.Interfaces.Service.PermissionTypes;
using isc.time.report.be.domain.Entity.Permisions;
using isc.time.report.be.domain.Models.Request.PermissionTypes;
using isc.time.report.be.domain.Models.Response.PermissionTypes;

namespace isc.time.report.be.application.Services.PermissionTypes
{
    public class PermissionTypeService : IPermissionTypeService
    {
        private readonly IPermissionTypeRepository permissionTypeRepository;
        private readonly IMapper _mapper;

        public PermissionTypeService(IPermissionTypeRepository repository, IMapper mapper)
        {
            permissionTypeRepository = repository;
            _mapper = mapper;
        }

        public async Task<List<GetPermissionTypeResponse>> GetAllPermissionTypesAsync()
        {
            var list = await permissionTypeRepository.GetAllPermissionTypeAsync();
            return _mapper.Map<List<GetPermissionTypeResponse>>(list);
        }

        public async Task<GetPermissionTypeResponse> GetPermissionTypeByIdAsync(int id)
        {
            var entity = await permissionTypeRepository.GetPermissionTypeByIdAsync(id);
            if (entity == null) throw new Exception("Tipo de permiso no encontrado");
            return _mapper.Map<GetPermissionTypeResponse>(entity);
        }

        public async Task<CreatePermissionTypeResponse> CreatePermissionTypeAsync(CreatePermissionTypeRequest request)
        {
            var entity = _mapper.Map<PermissionType>(request);
            var result = await permissionTypeRepository.CreatePermissionTypeAsync(entity);
            return _mapper.Map<CreatePermissionTypeResponse>(result);
        }

        public async Task<UpdatePermissionTypeResponse> UpdatePermissionTypeAsync(int id, UpdatePermissionTypeRequest request)
        {
            var entity = await permissionTypeRepository.GetPermissionTypeByIdAsync(id);
            if (entity == null) throw new Exception("Tipo de permiso no encontrado");

            _mapper.Map(request, entity);
            var result = await permissionTypeRepository.UpdatePermissionTypeAsync(entity);
            return _mapper.Map<UpdatePermissionTypeResponse>(result);
        }

        public async Task<ActiveInactivePermissionTypeResponse> InactivatePermissionTypeAsync(int id)
        {
            var result = await permissionTypeRepository.InactivatePermissionTypeAsync(id);
            return _mapper.Map<ActiveInactivePermissionTypeResponse>(result);
        }

        public async Task<ActiveInactivePermissionTypeResponse> ActivatePermissionTypeAsync(int id)
        {
            var result = await permissionTypeRepository.ActivatePermissionTypeAsync(id);
            return _mapper.Map<ActiveInactivePermissionTypeResponse>(result);
        }
    }
}
