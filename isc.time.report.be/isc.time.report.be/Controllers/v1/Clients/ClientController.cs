using isc.time.report.be.application.Interfaces.Service.Clients;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Clients;
using isc.time.report.be.domain.Models.Response.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Clients
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("GetAllClients")]
        public async Task<ActionResult<SuccessResponse<PagedResult<GetClientsDetailsResponse>>>> GetAllClients([FromQuery] PaginationParams paginationParams)
        {
            var result = await _clientService.GetAllClientsPaginated(paginationParams);
            return Ok(result);
        }

        [HttpGet("GetClientByID/{id}")]
        public async Task<ActionResult<SuccessResponse<GetClientsDetailsResponse>>> GetClientById(int id)
        {
            var result = await _clientService.GetClientByID(id);
            return Ok(result);
        }

        [HttpPost("CreateClientWithPersonID")]
        public async Task<ActionResult<SuccessResponse<CreateClientResponse>>> CreateClientWithPersonID([FromBody] CreateClientWithPersonIDRequest request)
        {
            var result = await _clientService.CreateClientWithPersonID(request);
            return Ok(result);
        }

        [HttpPost("CreateClientWithPerson")]
        public async Task<ActionResult<SuccessResponse<CreateClientResponse>>> CreateClientWithPerson([FromBody] CreateClientWithPersonOBJRequest request)
        {
            var result = await _clientService.CreateClientWithPerson(request);
            return Ok(result);
        }

        [HttpPut("UpdateClientWithPersonID/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdateClientResponse>>> UpdateClientWithPersonID(int id, [FromBody] UpdateClientWithPersonIDRequest request)
        {
            var result = await _clientService.UpdateClient(id, request);
            return Ok(result);
        }

        [HttpPut("UpdateClientWithPerson/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdateClientResponse>>> UpdateClientWithPerson(int id, [FromBody] UpdateClientWithPersonOBJRequest request)
        {
            var result = await _clientService.UpdateClientWithPerson(id, request);
            return Ok(result);
        }

        [HttpDelete("InactiveClientByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactiveResponse>>> InactivateClient(int id)
        {
            var result = await _clientService.InactivateClient(id);
            return Ok(result);
        }

        [HttpDelete("ActiveClientByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactiveResponse>>> ActivateClient(int id)
        {
            var result = await _clientService.ActivateClient(id);
            return Ok(result);
        }
    }
}

