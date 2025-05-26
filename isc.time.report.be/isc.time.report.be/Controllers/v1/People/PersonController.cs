using isc.time.report.be.application.Interfaces.Service.People;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.People;
using isc.time.report.be.domain.Models.Response.People;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.People
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/person")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<SuccessResponse<CreatePersonResponse>>> CreatePerson(CreatePersonRequest createRequest)
        {
            var person = await _personService.Create(createRequest);

            return Ok(new SuccessResponse<CreatePersonResponse>());
        }
    }
}
