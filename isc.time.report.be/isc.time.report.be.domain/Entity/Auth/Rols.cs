using isc.time.report.be.domain.Entity.Menu;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Auth
{
    public class Rols : BaseEntity
    {
        public string RolName { get; set; }
        public ICollection<UsersRols> UsersRols { get; set; }
        public ICollection<MenuRols> MenuRols { get; set; }

    }
}
