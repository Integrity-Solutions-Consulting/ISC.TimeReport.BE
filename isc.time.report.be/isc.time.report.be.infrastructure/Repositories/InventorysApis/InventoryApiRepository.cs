using DocumentFormat.OpenXml.Office2010.Excel;
using isc.time.report.be.application.Interfaces.Repository.InventoryApis;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysCustomers;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysEmployee;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysLogin;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysSuppliers;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils;
using isc.time.report.be.infrastructure.Utils.Peticiones;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly DBContext _dbContext;
        private readonly JWTInventoryUtils _jwtInventoryUtils;
        private readonly IConfiguration _configuration;
        public InventoryApiRepository(HttpUtils httpUtils, DBContext dBContext, JWTInventoryUtils jWTInventoryUtils, IConfiguration configuration)
        {
            _httpUtils = httpUtils;
            _dbContext = dBContext;
            _jwtInventoryUtils = jWTInventoryUtils;
            _configuration = configuration;
        }

        public async Task<string> LoginInventory()
        {
            var inventoryToken = await _dbContext.InventoryTokens.FirstOrDefaultAsync();

            if (inventoryToken == null)
            {
                inventoryToken = new InventoryToken();
                inventoryToken.Token = string.Empty;
                _dbContext.InventoryTokens.Add(inventoryToken);
                await _dbContext.SaveChangesAsync();
            }

            if (!string.IsNullOrWhiteSpace(inventoryToken.Token))
            {
                try
                {

                    var expirationDate = _jwtInventoryUtils.GetExpirationDateFromToken(inventoryToken.Token);

                    if (expirationDate > DateTime.UtcNow)
                        return inventoryToken.Token; 
                }
                catch
                {
                }
            }

            var request = new InventoryLoginDto
            {
                email = _configuration["InventoryCredentials:Email"],
                password = _configuration["InventoryCredentials:Password"],
               
            };


            var response = await _httpUtils.SendRequest<InventoryLoginResponse>("https://auth.inv.dokploy.integritysolutions.com.ec/api/v1/auth/login", HttpMethod.Post, request);


            if (response?.data?.token == null)
                throw new ClientFaultException("No se pudo obtener el token del servicio de inventario.");

            inventoryToken.Token = response.data.token;

            _dbContext.Entry(inventoryToken).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();


            return inventoryToken.Token;
        }

        public async Task<bool> CreateEmployeeInventoryAsync(InventoryCreateEmployeeRequest request)
        {
            var token = await LoginInventory();

            var response = await _httpUtils.SendRequest<object>($"{_configuration["Infrastructure:InventoryAPIUrlBase"]}/api/v1/employee/save", HttpMethod.Post, request, token);

            return response != null;
        }

        public async Task<bool> UpdateEmployeeInventoryAsync(InventoryUpdateEmployeeRequest request, string id)
        {
            var token = await LoginInventory();

            var response = await _httpUtils.SendRequest<object>($"{_configuration["Infrastructure:InventoryAPIUrlBase"]}/api/v1/employee/update/{id}", HttpMethod.Put, request, token);

            return response != null;
        }

        public async Task<bool> InactivateStatusEmployeeInventoryAsync(string id)
        {
            var token = await LoginInventory();

            var response = await _httpUtils.SendRequest<object>($"{_configuration["Infrastructure:InventoryAPIUrlBase"]}/api/v1/employee/inactive/{id}", HttpMethod.Delete, null, token);

            return response != null;
        }

        public async Task<bool> ActivateStatusEmployeeInventoryAsync(string id)
        {
            var token = await LoginInventory();

            var response = await _httpUtils.SendRequest<object>($"{_configuration["Infrastructure:InventoryAPIUrlBase"]}/api/v1/employee/activate/{id}", HttpMethod.Put, null, token);
            return response != null;
        }

        public async Task<SupplierResponseDto> GetInventoryProviders()
        {
            var token = await LoginInventory();

            var response = await _httpUtils.SendRequest<SupplierResponseDto>($"{_configuration["Infrastructure:InventoryAPIUrlBase"]}/api/v1/supplier/supplierType/2", HttpMethod.Get, null, token);

            return response ;
        }
        public async Task<bool> CreateCustomerInventoryAsync(InventoryCreateCustomerRequest request)
        {
            var token = await LoginInventory();
            var url = $"{_configuration["Infrastructure:InventoryAPIUrlBase"]}/api/v1/customers/save";
            var result = await _httpUtils.SendRequest<object>(url, HttpMethod.Post, request, token);
            return result != null;
        }

        public async Task<bool> UpdateCustomerInventoryAsync(InventoryUpdateCustomerRequest request, string id)
        {
            var token = await LoginInventory();
            var url = $"{_configuration["Infrastructure:InventoryAPIUrlBase"]}/api/v1/customers/update/{id}";
            var result = await _httpUtils.SendRequest<object>(url, HttpMethod.Put, request, token);
            return result != null;

        }

        public async Task<bool> InactivateCustomerInventoryAsync(string id)
        {
            var token = await LoginInventory();
            var url = $"{_configuration["Infrastructure:InventoryAPIUrlBase"]}/api/v1/customers/inactive/{id}";
            var result = await _httpUtils.SendRequest<object>(url, HttpMethod.Delete, null, token);
            return result != null;
        }

        public async Task<bool> ActivateCustomerInventoryAsync(string id)
        {
            var token = await LoginInventory();
            var url = $"{_configuration["Infrastructure:InventoryAPIUrlBase"]}/api/v1/customers/activate/{id}";
            var result = await _httpUtils.SendRequest<object>(url, HttpMethod.Patch, null, token);
            return result != null;
        }

    }
}
