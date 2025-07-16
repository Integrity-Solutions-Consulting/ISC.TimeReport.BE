using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Catalogs
{
    public class GetApprovalStatusResponse
    {
        public int Id { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string? Description { get; set; }
    }
}
