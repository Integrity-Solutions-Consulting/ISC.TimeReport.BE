using isc.time.report.be.application.Interfaces.Service.Leaders;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Request.Persons;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Leader
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/leader")]
    public class LeaderController : ControllerBase
    {
        private readonly ILeaderService _leaderService;

        public LeaderController(ILeaderService personService)
        {
            _leaderService = personService;
        }

        /*[HttpGet("get")]
        public async Task<ActionResult<SuccessResponse<GetLeaderResponse>>> GetAll()
        {
            var leader = await _leaderService.GetAll();
            return Ok(leader);
        }*/

        /*[HttpPost("create")]
        public async Task<ActionResult<SuccessResponse<CreateLeaderWithPersonResponse>>> CreateLeader(CreateLeaderWithPersonRequest createRequest)
        {
            var leader = await _leaderService.Create(createRequest);

            return Ok(new SuccessResponse<CreateLeaderResponse>());
        }*/

        /*[HttpPut("update/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdateLeaderResponse>>> UpdateLeader(int id, UpdateLeaderRequest updateLeaderRequest)
        {
            if (id != updateLeaderRequest.Id)
            {
                return BadRequest("ID no coincide");
            }

            var response = await _leaderService.Update(updateLeaderRequest);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }*/
    }
}
