using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Menu
{
    public class MenuRols : BaseEntity
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public int RolsId { get; set; }
        public Rols Rols { get; set; }

    }
}
