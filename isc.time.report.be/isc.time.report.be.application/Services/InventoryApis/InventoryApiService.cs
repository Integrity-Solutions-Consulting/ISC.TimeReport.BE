using isc.time.report.be.application.Interfaces.Repository.InventoryApis;
using isc.time.report.be.application.Interfaces.Service.InventoryApis;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysSuppliers;

namespace isc.time.report.be.application.Services.InventoryApis
{
    public class InventoryApiService : IInventoryApiService
    {
        private readonly IInventoryApiRepository _inventoryApiRepository;
        public InventoryApiService(IInventoryApiRepository inventoryApiRepository)
        {
            _inventoryApiRepository = inventoryApiRepository;
        }



        public async Task<SupplierResponseDto> GetInventoryProvidersType2()
        {

            var result = await _inventoryApiRepository.GetInventoryProviders();

            return result;
        }



    }
}
