using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Shared
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool Status { get; set; } = true;
        public string CreationUser { get; set; } = "SYSTEM";
        public string? ModificationUser { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? ModificationDate { get; set; }
        public string CreationIp { get; set; } = "0.0.0.0";
        public string? ModificationIp { get; set; }
    }
}