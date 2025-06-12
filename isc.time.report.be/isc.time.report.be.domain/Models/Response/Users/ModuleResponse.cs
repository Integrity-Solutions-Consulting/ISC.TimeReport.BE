using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Users
{
    public class ModuleResponse
    {
        public int Id { get; set; }
        public string ModuleName { get; set; } = null!;
        public string? ModulePath { get; set; }
        public string? Icon { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
