using isc.time.report.be.application.Interfaces.Service.Persons;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Request.Persons;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Persons
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

        [HttpGet("get")]
        public async Task<ActionResult<SuccessResponse<GetPersonListResponse>>> GetAll()
        {
            var person = await _personService.GetAll();
            return Ok(person);
        }

        [HttpPost("create")]
        public async Task<ActionResult<SuccessResponse<CreatePersonResponse>>> CreatePerson(CreatePersonRequest request)
        {
            var person = await _personService.Create(request);

            return Ok(new SuccessResponse<CreatePersonResponse>());
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdatePersonResponse>>> UpdateLeader(int id, UpdatePersonRequest updatePersonRequest)
        {
            if (id != updatePersonRequest.Id)
            {
                return BadRequest("ID no coincide");
            }

            var response = await _personService.Update(updatePersonRequest);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }
    }
}
