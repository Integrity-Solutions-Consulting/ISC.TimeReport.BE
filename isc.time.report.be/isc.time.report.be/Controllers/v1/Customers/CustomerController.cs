using isc.time.report.be.application.Interfaces.Service.Customers;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Customers;
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
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<SuccessResponse<CreateResponse>>> CreateCustomer(CreateRequest createRequest)
        {
            var customer = await customerService.CreateCustomer(createRequest);

            return Ok(new SuccessResponse<CreateResponse>());
        }
    }
}
