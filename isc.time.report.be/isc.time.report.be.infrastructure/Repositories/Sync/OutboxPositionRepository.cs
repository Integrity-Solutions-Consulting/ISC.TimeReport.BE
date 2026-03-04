using isc.time.report.be.domain.Entity.Sync;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Sync
{
    public class OutboxPositionRepository
    {
        private readonly DBContext _dbContext;
        public OutboxPositionRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<OutboxPosition>> GetPendingOutboxPositionsAsync()
        {
            return await _dbContext.OutboxPositions
               .Where(x => x.ProcessedAt == null
                        && x.NextAttemptAt <= DateTime.UtcNow)
               .OrderBy(x => x.CreatedAt)
               .Take(100)
               .ToListAsync();
        }

        public async Task MarkAsProcessedAsync(Guid outboxId)
        {
            var record = await _dbContext.OutboxPositions.FindAsync(outboxId);
            if (record is null) return;

            record.ProcessedAt = DateTime.UtcNow;
            record.ErrorMessage = null;
            await _dbContext.SaveChangesAsync();
        }

        public async Task MarkAsFailedAsync(Guid outboxId, string errorMessage)
        {
            var record = await _dbContext.OutboxPositions.FindAsync(outboxId);
            if (record is null) return;

            record.Attempts++;
            record.ErrorMessage = errorMessage;

            // Reintento: 5min → 10min → 20min → 40min...
            var minutesDelay = Math.Pow(2, record.Attempts) * 5;
            record.NextAttemptAt = DateTime.UtcNow.AddMinutes(minutesDelay);

            await _dbContext.SaveChangesAsync();
        }

    }
}
