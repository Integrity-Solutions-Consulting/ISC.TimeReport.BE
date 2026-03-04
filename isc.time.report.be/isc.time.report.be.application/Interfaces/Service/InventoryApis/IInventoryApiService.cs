using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysSuppliers;

namespace isc.time.report.be.application.Interfaces.Service.InventoryApis
{
    public interface IInventoryApiService
    {
        Task<SupplierResponseDto> GetInventoryProvidersType2();
    }
}
