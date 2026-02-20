using isc.time.report.be.application.Interfaces.Service.InventoryApis;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysSuppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.InventoryApis
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryApiController : ControllerBase
    {
        private readonly IInventoryApiService _inventoryApiService;
        public InventoryApiController(IInventoryApiService inventoryApiService)
        {
            _inventoryApiService = inventoryApiService;
        }
        [HttpGet("GetInventoryProviders")]
        public async Task<ActionResult<SupplierResponseDto>> GetInventoryProviders()
        {
            var result = await _inventoryApiService.GetInventoryProvidersType2();
            return Ok(result);
        }
    }
}
