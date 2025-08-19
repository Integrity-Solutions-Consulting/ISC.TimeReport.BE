using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Users
{
    public class RoleResponse
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
