﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Catalogs
{
    public class GetProjectTypeResponse
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = null!;
        public bool? SubType { get; set; }
    }
}
