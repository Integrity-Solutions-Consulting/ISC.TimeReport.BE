using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Auth
{
    public class RegisterRequest
    {
        public string email { get; set; }
        public string Password { get; set; }

        /*
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        */

    }
}
