using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Users
{
    public class UpdateUserRequest
    {
        public int EmployeeID { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
    }
}
