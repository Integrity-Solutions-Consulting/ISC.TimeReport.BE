using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Dto.InventorysApis.InventorySuppliers
{
    public class SupplierResponseDto
    {
        [JsonPropertyName("meta")]
        public MetaDto Meta { get; set; }

        [JsonPropertyName("data")]
        public List<SupplierDto> Data { get; set; }
    }

    public class MetaDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("pagination")]
        public object Pagination { get; set; }
    }

    public class SupplierDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("businessName")]
        public string BusinessName { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("taxId")]
        public string TaxId { get; set; }

        [JsonPropertyName("supplierType")]
        public SupplierTypeDto SupplierType { get; set; }
    }

    public class SupplierTypeDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
