using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysEmployee
{
    public class InventoryCreateEmployeeRequest
    {
        public int Id { get; set; }
        public int idIdentificationType { get; set; }
        public int idGender { get; set; }
        public int idPosition { get; set; }
        public int idWorkMode { get; set; }
        public int idNationality { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string identification { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public DateOnly contractDate { get; set; }
        public DateOnly? contractEndDate { get; set; }
    }
}
