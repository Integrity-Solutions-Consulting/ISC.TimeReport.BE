using isc.time.report.be.application.Interfaces.Service.InventoryApis;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorySuppliers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.InventoryApis
{
    [Route("api/[controller]")]
    [ApiController]
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
