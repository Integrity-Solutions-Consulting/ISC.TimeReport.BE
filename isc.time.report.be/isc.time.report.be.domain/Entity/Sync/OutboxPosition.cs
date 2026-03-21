using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Entity.Sync
{
    public class OutboxPosition
    {
        public Guid OutboxId { get; set; }
        public int AggregateKey { get; set; }
        public string Operation { get; set; }
        public string? PayloadJson { get; set; }
        public short Attempts { get; set; }
        public DateTime NextAttemptAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
