using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Auth
{
    public class User : BaseEntity
    {

        public string Username { get; set; }
        public string Password { get; set; }

        /*
        public string Names { get; set; }
        public string Lastnames { get; set; }
        public string Email { get; set; }
        public DateTime? LastLogin { get; set; }

        */


    }
}
