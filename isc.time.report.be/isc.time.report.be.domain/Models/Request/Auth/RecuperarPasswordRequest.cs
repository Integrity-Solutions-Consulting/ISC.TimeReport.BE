using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Auth
{
    public class RecuperarPasswordRequest
    {
        public string Username { get; set; } = null!;
    }
}
