using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Users
{
    public class UpdateUserRequest
    {
        public string email { get; set; }
        public string Password { get; set; }
    }
}
