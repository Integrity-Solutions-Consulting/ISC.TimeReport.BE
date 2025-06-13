using isc.time.report.be.application.Interfaces.Service.Customers;
using isc.time.report.be.application.Services.Clients;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Response.Customers;
using isc.time.report.be.domain.Models.Response.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Customers
{

    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly IClientService customerService;
        public CustomerController(IClientService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet("get")]
        public async Task<ActionResult<SuccessResponse<GetResponse>>> GetAllCustomers()
        {
            var customers = await customerService.GetAll();
            return Ok(customers);
        }

        [HttpPost("create")]
        public async Task<ActionResult<SuccessResponse<CreateCustomerResponse>>> CreateCustomer(CreateCustomerRequest createRequest)
        {
            var customer = await customerService.Create(createRequest);

            return Ok(new SuccessResponse<CreateCustomerResponse>());
        }

        [HttpPut("update/{id}")]
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
        }
    }
}
