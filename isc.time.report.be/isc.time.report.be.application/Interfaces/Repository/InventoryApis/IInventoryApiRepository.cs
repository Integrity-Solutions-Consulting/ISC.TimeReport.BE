using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysSuppliers;

namespace isc.time.report.be.application.Interfaces.Repository.InventoryApis
{
    public interface IInventoryApiRepository
    {
        Task<SupplierResponseDto> GetInventoryProviders();
    }
}
