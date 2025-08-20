using isc.time.report.be.application.Interfaces.Service.Persons;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Request.Persons;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Persons
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo")]
        [HttpGet("GetAllPersons")]
        public async Task<ActionResult<SuccessResponse<PagedResult<GetPersonResponse>>>> GetAllPersons(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? Search)
        {
            var result = await _personService.GetAllPersonsPaginated(paginationParams, Search);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo")]
        [HttpGet("GetPersonByID/{id}")]
        public async Task<ActionResult<SuccessResponse<GetPersonResponse>>> GetPersonById(int id)
        {
            var result = await _personService.GetPersonByID(id);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo")]
        [HttpPost("CreatePerson")]
        public async Task<ActionResult<SuccessResponse<CreatePersonResponse>>> CreatePerson([FromBody] CreatePersonRequest request)
        {
            var result = await _personService.CreatePerson(request);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo")]
        [HttpPut("UpdatePerson/{id}")]
        public async Task<ActionResult<SuccessResponse<UpdatePersonResponse>>> UpdatePerson(int id, [FromBody] UpdatePersonRequest request)
        {
            var result = await _personService.UpdatePerson(id, request);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo")]
        [HttpDelete("InactivatePersonByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactivePersonResponse>>> InactivatePerson(int id)
        {
            var result = await _personService.InactivatePerson(id);
            return Ok(result);
        }

        [Authorize(Roles = "Administrador,Gerente,Lider,Recursos Humanos,Administrativo")]
        [HttpDelete("ActivatePersonByID/{id}")]
        public async Task<ActionResult<SuccessResponse<ActiveInactivePersonResponse>>> ActivatePerson(int id)
        {
            var result = await _personService.ActivatePerson(id);
            return Ok(result);
        }
    }
}
