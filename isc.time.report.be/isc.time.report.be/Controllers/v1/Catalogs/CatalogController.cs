using isc.time.report.be.application.Interfaces.Service.Catalogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Catalogs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var result = await _catalogService.GetDepartmentsAsync();
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

        [HttpGet("project-type")]
        public async Task<IActionResult> GetProjectTypes()
        {
            var result = await _catalogService.GetProjectTypesAsync();
            return Ok(result);
        }

        [HttpGet("work-mode")]
        public async Task<IActionResult> GetWorkModes()
        {
            var result = await _catalogService.GetWorkModesAsync();
            return Ok(result);
        }

        [HttpGet("company-catalog")]
        public async Task<IActionResult> GetCompanyCatalog()
        {
            var result = await _catalogService.GetCompanyCatalogAsync();
            return Ok(result);
        }

        [HttpGet("employee-category")]
        public async Task<IActionResult> GetEmployeeCategory()
        {
            var result = await _catalogService.GetEmployeeCategoryAsync();
            return Ok(result);
        }
    }
}
