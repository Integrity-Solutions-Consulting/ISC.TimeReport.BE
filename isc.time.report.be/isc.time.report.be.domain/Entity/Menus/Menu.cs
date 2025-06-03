using isc.time.report.be.domain.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Menu
{
    public class Menu : BaseEntity
    {
        public string NombreMenu { get; set; }
        public string RutaMenu { get; set; }

        public ICollection<MenuRols> MenusRols { get; set; }
    }
}
