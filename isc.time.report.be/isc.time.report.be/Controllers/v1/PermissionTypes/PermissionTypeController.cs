﻿using isc.time.report.be.application.Interfaces.Service.PermissionTypes;
using isc.time.report.be.domain.Models.Request.PermissionTypes;
using isc.time.report.be.domain.Models.Response.PermissionTypes;
using isc.time.report.be.domain.Models.Response.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.PermissionTypes
{
    [ApiController]
    [Route("/timereport/api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class PermissionTypeController : ControllerBase
    {
        private readonly IPermissionTypeService _service;

        public PermissionTypeController(IPermissionTypeService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo,Colaborador")]
        [HttpGet("GetAllPermissionTypes")]
        public async Task<ActionResult<SuccessResponse<List<GetPermissionTypeResponse>>>> GetAll()
        {
            var result = await _service.GetAllPermissionTypesAsync();
            return Ok(new SuccessResponse<List<GetPermissionTypeResponse>>(200, "Tipos de permiso obtenidos correctamente", result));
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo,Colaborador")]
        [HttpGet("GetPermissionTypeByID/{id}")]
        public async Task<ActionResult<SuccessResponse<GetPermissionTypeResponse>>> GetById(int id)
        {
            var result = await _service.GetPermissionTypeByIdAsync(id);
            return Ok(new SuccessResponse<GetPermissionTypeResponse>(200, $"Tipo de permiso con ID {id} obtenido correctamente", result));
        }

        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost("CreatePermissionType")]
        public async Task<ActionResult<SuccessResponse<CreatePermissionTypeResponse>>> Create([FromBody] CreatePermissionTypeRequest request)
        {
            var result = await _service.CreatePermissionTypeAsync(request);
            return Ok(new SuccessResponse<CreatePermissionTypeResponse>(201, "Tipo de permiso creado exitosamente", result));
        }

        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPut("UpdatePermissionType/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdatePermissionTypeResponse>>> Update(int id, [FromBody] UpdatePermissionTypeRequest request)
        {
            var result = await _service.UpdatePermissionTypeAsync(id, request);
            return Ok(new SuccessResponse<UpdatePermissionTypeResponse>(200, $"Tipo de permiso con ID {id} actualizado correctamente", result));
        }

        [Authorize(Roles = "Administrador,Gerente")]
        [HttpDelete("InactivatePermissionType/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactivePermissionTypeResponse>>> Inactivate(int id)
        {
            var result = await _service.InactivatePermissionTypeAsync(id);
            return Ok(new SuccessResponse<ActiveInactivePermissionTypeResponse>(200, $"Tipo de permiso con ID {id} inactivado correctamente", result));
        }

        [Authorize(Roles = "Administrador,Gerente")]
        [HttpDelete("ActivatePermissionType/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactivePermissionTypeResponse>>> Activate(int id)
        {
            var result = await _service.ActivatePermissionTypeAsync(id);
            return Ok(new SuccessResponse<ActiveInactivePermissionTypeResponse>(200, $"Tipo de permiso con ID {id} activado correctamente", result));
        }
    }
}
