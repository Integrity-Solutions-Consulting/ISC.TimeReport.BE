using isc.time.report.be.application.Interfaces.Service.Clients;
using isc.time.report.be.application.Services.Clients;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Clients;
using isc.time.report.be.domain.Models.Response.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Clients
{

    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;
        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        /*[HttpGet("get")]
        public async Task<ActionResult<SuccessResponse<GetResponse>>> GetAllCustomers()
        {
            var customers = await customerService.GetAll();
            return Ok(customers);
        }*/

        [HttpPost("create")]
        public async Task<ActionResult<SuccessResponse<CreateClientResponse>>> CreateCustomer(CreateClientWithPersonRequest createRequest)
        {
            var customer = await clientService.Create(createRequest);

            return Ok(new SuccessResponse<CreateClientResponse>());
        }

        /*[HttpPut("update/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdateResponse>>> UpdateCustomer(int id, UpdateRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID no coincide");
            }

            var response = await customerService.Update(request);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }*/
    }
}
