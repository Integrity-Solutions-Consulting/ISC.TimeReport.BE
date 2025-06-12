using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Genders
{
    public class Gender : BaseEntity
    {
        public string GenderCode { get; set; } = null!;
        public string GenderName { get; set; } = null!;
    }
}
