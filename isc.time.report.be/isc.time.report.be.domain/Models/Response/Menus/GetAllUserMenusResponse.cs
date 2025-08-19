using isc.time.report.be.domain.Entity.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Menus
{
    public class GetAllUserMenusResponse
    {
        public int Id { get; set; }
        public string ModuleName {  get; set; }
        public string ModulePath { get; set; }
    }
}