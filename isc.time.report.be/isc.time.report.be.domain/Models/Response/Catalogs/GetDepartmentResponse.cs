using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Catalogs
{
    public class GetDepartmentResponse
    {
        public int Id { get; set; }
        public string DepartamentName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
