using isc.time.report.be.application.Interfaces.Service.Leaders;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.People
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/leader")]
    public class LeaderController : ControllerBase
    {
        private readonly ILeaderService _personService;

        public LeaderController(ILeaderService personService)
        {
            _personService = personService;
        }

        [HttpGet("get")]
        public async Task<ActionResult<SuccessResponse<GetLeaderResponse>>> GetAll()
        {
            var people = await _personService.GetAll();
            return Ok(people);
        }

        [HttpPost("create")]
        public async Task<ActionResult<SuccessResponse<CreateLeaderResponse>>> CreatePerson(CreateLeaderRequest createRequest)
        {
            var person = await _personService.Create(createRequest);

            return Ok(new SuccessResponse<CreateLeaderResponse>());
        }
    }
}
