using isc.time.report.be.application.Interfaces.Service.Leaders;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Leader
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderController : ControllerBase
    {
        private readonly ILeaderService _leaderService;

        public LeaderController(ILeaderService leaderService)
        {
            _leaderService = leaderService;
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("GetAllLeaders")]
        public async Task<ActionResult<SuccessResponse<PagedResult<GetLeaderDetailsResponse>>>> GetAllLeaders(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? search)
        {
            var result = await _leaderService.GetAllLeadersPaginated(paginationParams, search);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("GetLeaderByID/{id}")]
        public async Task<ActionResult<SuccessResponse<GetLeaderDetailsResponse>>> GetLeaderById(int id)
        {
            var result = await _leaderService.GetLeaderByID(id);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("get-leadership-by-person-id")]
        public async Task<ActionResult<SuccessResponse<GetAllLeaderProjectByPersonIdResponse>>> GetLeadershipByPerson(int id)
        {
            var result = await _leaderService.GetLeadershipByPersonId(id);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpGet("get-all-leaders-grouped")]
        public async Task<ActionResult<SuccessResponse<List<GetAllLeaderProjectByPersonIdResponse>>>> GetAllLeadersGrouped()
        {
            var result = await _leaderService.GetAllLeadersRegisterGrouped();
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPost("CreateLeaderWithPersonID")]
        public async Task<ActionResult<SuccessResponse<CreateLeaderResponse>>> CreateLeaderWithPersonID([FromBody] CreateLeaderWithPersonIDRequest request)
        {
            var result = await _leaderService.CreateLeaderWithPersonID(request);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPost("CreateLeaderWithPerson")]
        public async Task<ActionResult<SuccessResponse<CreateLeaderResponse>>> CreateLeaderWithPerson([FromBody] CreateLeaderWithPersonOBJRequest request)
        {
            var result = await _leaderService.CreateLeaderWithPerson(request);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPut("UpdateLeaderWithPersonID/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdateLeaderResponse>>> UpdateLeaderWithPersonID(int id, [FromBody] UpdateLeaderWithPersonIDRequest request)
        {
            var result = await _leaderService.UpdateLeader(id, request);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPut("UpdateLeaderWithPerson/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdateLeaderResponse>>> UpdateLeaderWithPerson(int id, [FromBody] UpdateLeaderWithPersonOBJRequest request)
        {
            var result = await _leaderService.UpdateLeaderWithPerson(id, request);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpPost("assign-leader-to-project")]
        public async Task<ActionResult> AssignedLeaderToProject ([FromBody] AssignPersonToProjectRequest request)
        {
            await _leaderService.AssignPersonToProject(request);
            return Ok(new { message = "Lideres asignados correctamente."});

        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpDelete("InactivateLeaderByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActivateInactivateLeaderResponse>>> InactivateLeader(int id)
        {
            var result = await _leaderService.InactivateLeader(id);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider")]
        [HttpDelete("ActivateLeaderByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActivateInactivateLeaderResponse>>> ActivateLeader(int id)
        {
            var result = await _leaderService.ActivateLeader(id);
            return Ok(result);
        }

    }
}
