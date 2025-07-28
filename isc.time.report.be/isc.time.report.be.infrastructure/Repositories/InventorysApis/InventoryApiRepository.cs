using isc.time.report.be.application.Interfaces.Repository.InventoryApis;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysEmployee;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysLogin;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorySuppliers;
using isc.time.report.be.infrastructure.Utils.Peticiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.InventorysApis
{
    public class InventoryApiRepository : IInventoryApiRepository
    {
        private readonly HttpUtils _httpUtils;
        public InventoryApiRepository(HttpUtils httpUtils)
        {
            _httpUtils = httpUtils;
        }

        public async Task<string> LoginInventory()
        {
            var request = new InventoryLoginDto
            {
                email = "admin@integrity.com",
                password = "Password2@"
            };

            var url = "https://isc-inventory-back.onrender.com/api/v1/auth/login";

            var response = await _httpUtils.SendRequest<InventoryLoginResponse>(url, HttpMethod.Post, request);

            if (response?.data?.token == null)
                throw new InvalidOperationException("No se pudo obtener el token del servicio de inventario.");

            return response.data.token;
        }

        public async Task<bool> CreateEmployeeInventoryAsync(InventoryCreateEmployeeRequest request)
        {
            var token = await LoginInventory();

            var url = "https://isc-inventory-back-api.onrender.com/api/v1/employee/save";

            var result = await _httpUtils.SendRequest<object>(url, HttpMethod.Post, request, token);

            return result != null;
        }

        public async Task<bool> UpdateEmployeeInventoryAsync(InventoryUpdateEmployeeRequest request, int id)
        {
            var token = await LoginInventory();

            var url = $"https://isc-inventory-back-api.onrender.com/api/v1/employee/Update/{id}";

            var result = await _httpUtils.SendRequest<object>(url, HttpMethod.Put, request, token);

            return result != null;
        }

        public async Task<bool> InactivateStatusInventoryAsync(int id)
        {
            var token = await LoginInventory();

            var url = $"https://isc-inventory-back-api.onrender.com/api/v1/employee/inactive/{id}";

            var result = await _httpUtils.SendRequest<object>(url, HttpMethod.Delete, null, token);
            return result != null;
        }

        public async Task<bool> ActivateStatusInventoryAsync(int id)
        {
            var token = await LoginInventory();

            var url = $"https://isc-inventory-back-api.onrender.com/api/v1/employee/activate/{id}";

            var result = await _httpUtils.SendRequest<object>(url, HttpMethod.Put, null, token);
            return result != null;
        }

        public async Task<SupplierResponseDto> GetInventoryProviders()
        {
            var token = await LoginInventory();

            var url = $"https://isc-inventory-back-api.onrender.com/api/v1/supplier/supplierType/2";

            var result = await _httpUtils.SendRequest<SupplierResponseDto>(url, HttpMethod.Get, null, token);

            return result;
        }

    }
}
