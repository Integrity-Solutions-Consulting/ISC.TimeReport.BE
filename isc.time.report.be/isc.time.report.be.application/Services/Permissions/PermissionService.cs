using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Permissions;
using isc.time.report.be.application.Interfaces.Service.Permissions;
using isc.time.report.be.domain.Entity.Permisions;
using isc.time.report.be.domain.Models.Request.Permissions;
using isc.time.report.be.domain.Models.Response.Permissions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.Permissions
{
    public class PermissionService: IPermissionService
    {
        private readonly IPermissionRepository permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository repository, IMapper mapper)
        {
            permissionRepository = repository;
            _mapper = mapper;
        }

        public async Task<CreatePermissionResponse> CreatePermissionAsync(CreatePermissionRequest request, int employeeId)
        {
            if (request.EndDate < request.StartDate)
                throw new ArgumentException("La fecha final no puede ser menor a la inicial.");

            var permission = _mapper.Map<Permission>(request);
            permission.EmployeeID = employeeId;
            permission.CreationDate = DateTime.Now;
            permission.CreationUser = "SYSTEM";
            permission.Status = true;
            permission.TotalDays = Math.Round((decimal)(request.EndDate.Date - request.StartDate.Date).TotalDays + 1, 2 );
            permission.TotalHours = Math.Round((decimal)(request.EndDate - request.StartDate).TotalHours, 2 );

            var created = await permissionRepository.CreateAsync(permission);
            return _mapper.Map<CreatePermissionResponse>(created);
        }

        public async Task<GetPermissionResponse> ApprovePermissionAsync(PermissionAproveRequest request, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("No se pudo obtener el UserID del token.");

            int approvedById = int.Parse(userIdClaim);

            var permissionOrig = await permissionRepository.GetPermissionByIdAsync(request.PermissionID);

            if (permissionOrig == null)
                throw new KeyNotFoundException($"No se encontró el permiso con ID {request.PermissionID}");

            permissionOrig.ApprovalStatus = request.ApprovalStatus;
            permissionOrig.ApprovedByID = approvedById;
            permissionOrig.ApprovalDate = DateTime.Now;
            permissionOrig.Observation = request.Observation;
            permissionOrig.ModificationDate = DateTime.Now;
            permissionOrig.ModificationUser = "SYSTEM";

            var updated = await permissionRepository.ApproveAsync(permissionOrig);
            return _mapper.Map<GetPermissionResponse>(updated);
        }

        public async Task<List<GetPermissionResponse>> GetAllPermissionAsync(ClaimsPrincipal user)
        {
            var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var idClaim = user.Claims.FirstOrDefault(c => c.Type == "EmployeeID")?.Value;
            var isAdmin = role == "Admin" || role == "Manager";
            int? employeeId = isAdmin ? null : int.Parse(idClaim);

            var list = await permissionRepository.GetAllAsync(employeeId, isAdmin);
            return list.Select(p => _mapper.Map<GetPermissionResponse>(p)).ToList();
        }
    }
}
