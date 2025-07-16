using isc.time.report.be.application.Interfaces.Service.Catalogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Catalogs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet("activity-types")]
        public async Task<IActionResult> GetActivityTypes()
        {
            var result = await _catalogService.GetActivityTypesAsync();
            return Ok(result);
        }

        [HttpGet("approval-statuses")]
        public async Task<IActionResult> GetApprovalStatuses()
        {
            var result = await _catalogService.GetApprovalStatusesAsync();
            return Ok(result);
        }

        [HttpGet("genders")]
        public async Task<IActionResult> GetGenders()
        {
            var result = await _catalogService.GetGendersAsync();
            return Ok(result);
        }

        [HttpGet("identification-types")]
        public async Task<IActionResult> GetIdentificationTypes()
        {
            var result = await _catalogService.GetIdentificationTypesAsync();
            return Ok(result);
        }

        [HttpGet("nationalities")]
        public async Task<IActionResult> GetNationalities()
        {
            var result = await _catalogService.GetNationalitiesAsync();
            return Ok(result);
        }

        [HttpGet("permission-types")]
        public async Task<IActionResult> GetPermissionTypes()
        {
            var result = await _catalogService.GetPermissionTypesAsync();
            return Ok(result);
        }

        [HttpGet("positions")]
        public async Task<IActionResult> GetPositions()
        {
            var result = await _catalogService.GetPositionsAsync();
            return Ok(result);
        }

        [HttpGet("project-statuses")]
        public async Task<IActionResult> GetProjectStatuses()
        {
            var result = await _catalogService.GetProjectStatusesAsync();
            return Ok(result);
        }

    }
}
