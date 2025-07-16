using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Catalogs
{
    public class GetPositionResponse
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        public string Department { get; set; }
        public string? Description { get; set; }
    }
}
