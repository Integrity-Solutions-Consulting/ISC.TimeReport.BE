using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Users
{
    public class GetAllUsersResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool Status { get; set; }
        public List<string> Roles { get; set; }
    }
}
