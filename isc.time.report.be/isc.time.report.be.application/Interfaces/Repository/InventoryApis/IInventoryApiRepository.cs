using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysSuppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Repository.InventoryApis
{
    public interface IInventoryApiRepository
    {
        Task<SupplierResponseDto> GetInventoryProviders();
    }
}
