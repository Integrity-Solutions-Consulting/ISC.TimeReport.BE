using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Menu;
using isc.time.report.be.domain.Entity.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace isc.time.report.be.domain.Entity.Auth
{
    public class User : BaseEntity
    {

        public string email { get; set; }
        public string Password { get; set; }
        public ICollection<UsersRols> UsersRols { get; set; }
    }
}
